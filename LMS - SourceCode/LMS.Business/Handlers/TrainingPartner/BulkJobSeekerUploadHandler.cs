using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Shared;
using LMS.Business.Interfaces.TrainingPartner;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Auth;
using LMS.Model.DataViewModel.Shared;
using LMS.Model.DataViewModel.TrainingPartner;
using LMS.Utility.Exceptions;
using LMS.Utility.FilesUtility;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace LMS.Business.Handlers.TrainingPartner
{
    public class BulkJobSeekerUploadHandler : IBulkJobSeekerUploadHandler
    {
        private readonly IHostingEnvironment environment;
        private readonly IAuthRepository authProcessor;
        private readonly IEMailHandler emailHandler;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string URLprotocol;

        public BulkJobSeekerUploadHandler(IHostingEnvironment env, IConfiguration configuration, IEMailHandler _emailHandler, IHttpContextAccessor httpContextAccessor)
        {
            environment = env;
            var authFactory = new ProcessorFactoryResolver<IAuthRepository>(configuration);
            authProcessor = authFactory.CreateProcessor();
            emailHandler = _emailHandler;
            config = configuration;
            _httpContextAccessor = httpContextAccessor;
            URLprotocol = configuration["URLprotocol"];
        }

        public IEnumerable<BulkUploadSummaryViewModel<BulkJobSeekerUploadSummaryViewModel>> RegisterJobSeekers(UserViewModel user, IList<IFormFile> files)
        {
            string roleName=user.RoleName;
            string mappingFilePath =
                Path.Combine(environment.WebRootPath, "DataMappings", "BulkJobSeekerMappings", "JobSeekerMapping.xml");
            DataTable dt = XmlProcessor.XmlToTable(mappingFilePath);
            if (null != dt)
            {
                var summary = new List<BulkUploadSummaryViewModel<BulkJobSeekerUploadSummaryViewModel>>();
                foreach (var file in files)
                {
                    var fileSummary = new BulkUploadSummaryViewModel<BulkJobSeekerUploadSummaryViewModel>
                    {
                        FileName = file.FileName,
                        Summary = new List<BulkJobSeekerUploadSummaryViewModel>()
                    };
                    DataTable dTable = null;
                    NPOIManager.ReadFile(file, dt, out dTable, true, roleName);
                    if (null != dTable && dTable.Rows.Count > 1)
                    {
                        string[] additionalColumns = new string[] { "ProcessedBy", "ProcessedOn", "Status", "ErrorDetails" };
                        //  If at the time of file read we pass true for first row as header then here also we need to pass the same.
                        ExtendTable(additionalColumns, ref dTable, user.UserId, true);

                        UserModel jsDetail = null;
                        int rIndex = -1;
                        foreach (DataRow row in dTable.Rows)
                        {
                            rIndex++;
                            if (rIndex == 0)
                            {
                                fileSummary.Summary.Add(AddSummary(row));
                                continue;
                            }

                            try
                            {
                                row[additionalColumns[0]] = user.FullName;
                                row[additionalColumns[1]] = DateTime.Now;
                                ResolveJobSeekerDetail(out jsDetail, row, additionalColumns);
                                foreach (DataColumn col in dTable.Columns)
                                {
                                    //  Check if any required field is empty
                                    switch (col.ColumnName)
                                    {
                                        case "CandidateId":
                                        case "FirstName":
                                        case "LastName":
                                        case "Email":
                                            if (string.IsNullOrEmpty(Convert.ToString(row[col.ColumnName])))
                                            {
                                                row[additionalColumns[2]] = "Failed";
                                                row[additionalColumns[3]] += $"<li>{dTable.Rows[0][col.ColumnName]} is mandatory</li>";
                                            }
                                            break;
                                    }

                                    //  Validate User Info
                                    switch (col.ColumnName)
                                    {
                                        //  User already exist or not
                                        case "Email":
                                            if (authProcessor.CheckIfUserExists(Convert.ToString(row[col.ColumnName])))
                                            {
                                                row[additionalColumns[2]] = "Failed";
                                                row[additionalColumns[3]] += "<li>Candidate is already registered in Placement Portal</li>";
                                            }
                                            else
                                            {
                                                jsDetail.IsActive = false;
                                                jsDetail.ActivationKey = RandomGenerator.GetRandom(20);
                                                jsDetail.Password= RandomGenerator.GetRandom();
                                            }
                                            break;
                                        //  Candidate Id already exist or not
                                        case "CandidateId":
                                            if (authProcessor.CheckCandidateIdExist(Convert.ToString(row[col.ColumnName])))
                                            {
                                                row[additionalColumns[2]] = "Failed";
                                                row[additionalColumns[3]] += "<li>Candidate Id already exist in our system.</li>";
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    DataRow r = authProcessor.CandidateResult(Convert.ToString(row[col.ColumnName]));
                                                    if (null != r)
                                                    {
                                                        jsDetail.GetType().GetProperty("BatchNumber").SetValue(jsDetail, r["BatchNumber"]);
                                                    }
                                                }
                                                catch (DataNotFound ex)
                                                {
                                                    row[additionalColumns[2]] = "Failed";
                                                    row[additionalColumns[3]] += $"<li>{ex.Message}</li>";
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if ("Failed" != Convert.ToString(row[additionalColumns[2]]))
                                {
                                    jsDetail.CreatedBy = Convert.ToInt32(user.UserId);
                                    jsDetail.RoleId = 2;
                                    int userId = authProcessor.RegisterUser(jsDetail);
                                    if (userId > 0)
                                    {
                                        try
                                        {
                                            jsDetail.UserId = userId;
                                            SendActivationMail(jsDetail);
                                        }
                                        catch (Exception ex)
                                        {
                                            row[additionalColumns[2]] = "Failed";
                                            row[additionalColumns[3]] += "<li>Unable to send email verification link to user, Please contact your tech deck.</li>";
                                        }
                                        row[additionalColumns[2]] = "Success";
                                    }
                                    else
                                    {
                                        row[additionalColumns[2]] = "Failed";
                                        row[additionalColumns[3]] += "<li>Unable to register this user, Please contact your tech deck.</li>";
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                row[additionalColumns[2]] = "Failed";
                                row[additionalColumns[3]] += $"<li>{ex.Message}</li>";
                            }

                            fileSummary.Summary.Add(AddSummary(row));
                        }

                    }
                    else
                    {
                        throw new DataNotFound(
                            string.Format("{0}", "Data not found in file to import."));
                    }
                    summary.Add(fileSummary);

                }
                return summary;
            }
            else
            {
                throw new XmlFileMapperException(
                    string.Format("{0}", "JobSeekerMapping.xml file is not in proper format to convert in data table."));
            }
        }

        #region Private Methods

        private void ExtendTable(string[] additionalColumns, ref DataTable table, int userId, bool isFirstRowAlsoAsHeader = false)
        {
            int len = additionalColumns.Length;

            for (int i = 0; i < len; i++)
            {
                //  Adding additional Columns
                table.Columns.Add(additionalColumns[i]);
                if (isFirstRowAlsoAsHeader)
                {
                    table.Rows[0][additionalColumns[i]] = additionalColumns[i];
                }
            }
        }

        private BulkJobSeekerUploadSummaryViewModel AddSummary(DataRow row)
        {
            var summary = new BulkJobSeekerUploadSummaryViewModel();
            var pInfos = summary.GetType().GetProperties();

            foreach (var pInfo in pInfos)
            {
                pInfo.SetValue(summary, Convert.ToString(row[pInfo.Name]));
            }
            return summary;
        }

        private void ResolveJobSeekerDetail(out UserModel jSeeker, DataRow row, string[] additionalColumns)
        {
            jSeeker = new UserModel();
            var pInfos = jSeeker.GetType().GetProperties();

            foreach (var pInfo in pInfos)
            {
                try
                {
                    DataColumnCollection columns = row.Table.Columns;
                    if (columns.Contains(pInfo.Name))
                    {
                        switch (pInfo.PropertyType.Name)
                        {
                            case "String":
                                pInfo.SetValue(jSeeker, Convert.ToString(row[pInfo.Name]));
                                break;
                            case "Int32":
                                pInfo.SetValue(jSeeker, Convert.ToInt32(row[pInfo.Name]));
                                break;
                            default:
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>{Convert.ToString(row[pInfo.Name])} is not valid data, expecting {pInfo.PropertyType.Name} type of data.</li>";
                }
            }
        }

        private void SendActivationMail(UserModel user)
        {
            string activationLink =
                $"{URLprotocol}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host.Value}" +
                $"/Auth/EmailVerification?uId={user.UserId}&akey={user.ActivationKey}";

            var eModel = new EmailViewModel
            {
                Subject = "Account Activation Link",
                Body = $"Dear {user.FirstName},<br/>Congrat's you have successfully registered with us." +
                $"You are one step away to explore our application," +
                $"Please <a href={activationLink}>click here</a> to activate your account." +
                $"Your login details are below:<br/><br/>User Name:  {user.Email}<br/>Password: {user.Password} " +
                $"<br/><br/>Thank You <br/> Placement Portal Team",

                To = new string[] { user.Email },
                From = config["EmailCredential:Fromemail"],
                IsHtml = true,
                MailType = (int)MailType.UserRegistrationActivationLink
            };
            emailHandler.SendMail(eModel, -1);
        }

        #endregion
    }
}
