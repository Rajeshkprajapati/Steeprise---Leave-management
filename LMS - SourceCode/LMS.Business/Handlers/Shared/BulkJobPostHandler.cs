using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Shared;
using LMS.Data.DataModel.Employer.JobPost;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Auth;
using LMS.Data.Interfaces.Employer.JobPost;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Shared;
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
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMS.Business.Handlers.Shared
{
    public class BulkJobPostHandler : IBulkJobPostHandler
    {
        private readonly IHostingEnvironment environment;
        private readonly IBulkJobPostRepository bjpProcessor;
        private readonly IAuthRepository authProcessor;
        private readonly IJobPostRepository jobPostProcessor;
        private readonly IEMailHandler emailHandler;
        public BulkJobPostHandler(IHostingEnvironment env, IConfiguration configuration, IEMailHandler _emailHandler)
        {
            environment = env;
            emailHandler = _emailHandler;
            var authFactory = new ProcessorFactoryResolver<IAuthRepository>(configuration);
            authProcessor = authFactory.CreateProcessor();
            var bjpFactory = new ProcessorFactoryResolver<IBulkJobPostRepository>(configuration);
            bjpProcessor = bjpFactory.CreateProcessor();
            var jpFactory = new ProcessorFactoryResolver<IJobPostRepository>(configuration);
            jobPostProcessor = jpFactory.CreateProcessor();
        }

        public Task UploadJobsInBackground(UserViewModel user, IList<IFormFile> files)
        {
            //await Task.Run(() =>
            //{
            //    UploadJobs(user,files);
            //});
            throw new NotImplementedException();

        }

        public IEnumerable<BulkUploadSummaryViewModel<BulkJobPostSummaryDetailViewModel>> UploadJobs(UserViewModel user, IList<IFormFile> files)
        {
            string roleName = user.RoleName;
            string mappingFilePath =
                Path.Combine(environment.WebRootPath, "DataMappings", "BulkJobPostMappings", "JobPostMapping.xml");
            DataTable dt = XmlProcessor.XmlToTable(mappingFilePath);
            if (null != dt)
            {
                var summary = new List<BulkUploadSummaryViewModel<BulkJobPostSummaryDetailViewModel>>();
                foreach (var file in files)
                {
                    var fileSummary = new BulkUploadSummaryViewModel<BulkJobPostSummaryDetailViewModel>
                    {
                        FileName = file.FileName,
                        Summary = new List<BulkJobPostSummaryDetailViewModel>()
                    };
                    DataTable dTable = null;
                    NPOIManager.ReadFile(file, dt, out dTable, true, roleName);
                    if (null != dTable && dTable.Rows.Count > 1)
                    {
                        string[] additionalColumns = new string[] { "ProcessedBy", "ProcessedOn", "Status", "ErrorDetails" };
                        //  If at the time of file read we pass true for first row as header then here also we need to pass the same.
                        ExtendTable(additionalColumns, ref dTable, user.UserId, true);

                        //  In Case of multiple cities in Job Location Column we need to divide them in multiple rows as below.
                        SimpliFyTableDataForMultipleCitiesForSingleJob(dTable);
                        JobPostModel jDetail = null;
                        int rIndex = -1;
                        foreach (DataRow row in dTable.Rows)
                        {
                            rIndex++;
                            if (rIndex == 0)
                            {
                                fileSummary.Summary.Add(AddSummary(row));
                                continue;
                            }
                            BulkJobPostSummaryDetailViewModel sData = null;
                            row[additionalColumns[0]] = user.FullName;
                            row[additionalColumns[1]] = DateTime.Now;

                            //  Checking if user authorized to post jobs.
                            if (!IsAllowToPostJob(user, row))
                            {
                                row[additionalColumns[2]] = "Failed";
                                row[additionalColumns[3]] += $"<li>You are not allowed to post this job - Enter company name used while registration.</li>";
                                sData = AddSummary(row);
                                fileSummary.Summary.Add(sData);
                                SaveDetailToAudit(sData, fileSummary.FileName, user.UserId);
                                continue;
                            }
                            try
                            {
                                //Saving JobTitleByEmployer as JobTitle
                                GetJobTitle(row, additionalColumns);

                                ManageJobType(row, additionalColumns);

                                //  Checking if at least one quarter must have a value                         
                                //ValidateQuarters(row, additionalColumns);

                                //Validate SPOC Contact
                                ValidateSpocContact(row, additionalColumns);

                                // Validate Job Expiry Dates
                                ValidateJobExpiryDates(row, additionalColumns);

                                //   All external validations should be done before this method.
                                ResolveJobData(out jDetail, row, additionalColumns);

                                //  Validate model if some thing needs for default value.
                                ValidateJobModel(ref jDetail);

                                foreach (DataColumn col in dTable.Columns)
                                {
                                    //  Check if any required field is empty
                                    switch (col.ColumnName)
                                    {
                                        case "CompanyName":
                                        case "StateCode":
                                        case "CityCode":
                                        case "JobTitleByEmployer":
                                        case "JobTitleId":
                                        case "SPOC":
                                        case "SPOCEmail":
                                        case "SPOCContact":
                                        //case "CTC":
                                        case "HiringCriteria":
                                        //case "Quarter1":
                                        //case "Quarter2":
                                        //case "Quarter3":
                                        //case "Quarter4":
                                        case "JobType":
                                        case "PositionStartDate":
                                        case "PositionEndDate":
                                            if (string.IsNullOrEmpty(Convert.ToString(row[col.ColumnName])))
                                            {
                                                switch (col.ColumnName)
                                                {
                                                    case "JobTitleId":
                                                        row[additionalColumns[2]] = "Failed";
                                                        row[additionalColumns[3]] += $"<li>At least one job Title is mandatory</li>";
                                                        break;
                                                    default:
                                                        row[additionalColumns[2]] = "Failed";
                                                        row[additionalColumns[3]] += $"<li>{dTable.Rows[0][col.ColumnName]} is mandatory</li>";
                                                        break;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    //  Check if master information available in our system or not.
                                    DataTable t = null;
                                    switch (col.ColumnName)
                                    {
                                        //case "FinancialYear":
                                        //    int year = Convert.ToInt32(row[col.ColumnName]);
                                        //    if (year <= 0)
                                        //    {
                                        //        jDetail.GetType().GetProperty(col.ColumnName).SetValue(jDetail, DateTime.Now.Year);
                                        //    }
                                        //    break;
                                        case "StateCode":
                                            string state = Convert.ToString(row[col.ColumnName]);
                                            t =
                                                bjpProcessor.GetIdFromValue(state, col.ColumnName);
                                            if (null != t && t.Rows.Count > 0)
                                            {
                                                jDetail.GetType().GetProperty(col.ColumnName).SetValue(jDetail, t.Rows[0]["Id"]);
                                            }
                                            else
                                            {
                                                //var sModel = new StateModel
                                                //{
                                                //    CountryCode = jDetail.CountryCode,
                                                //    State = state,
                                                //    StateCode = string.Empty
                                                //};
                                                //bjpProcessor.InsertState(ref sModel);
                                                //if (!string.IsNullOrWhiteSpace(sModel.StateCode))
                                                //{
                                                //    jDetail.GetType().GetProperty(col.ColumnName).SetValue(jDetail, sModel.StateCode);
                                                //}
                                                //else
                                                //{
                                                row[additionalColumns[2]] = "Failed";
                                                row[additionalColumns[3]] += "<li>State Not Found In Our Record</li>";
                                                //}
                                            }

                                            break;
                                        case "CityCode":
                                            string city = Convert.ToString(row[col.ColumnName]);
                                            t =
                                                bjpProcessor.GetIdFromValue(city, col.ColumnName);
                                            if (null != t && t.Rows.Count > 0)
                                            {
                                                jDetail.GetType().GetProperty(col.ColumnName).SetValue(jDetail, t.Rows[0]["Id"]);
                                            }
                                            else
                                            {
                                                //var cModel = new CityModel
                                                //{
                                                //    City = city,
                                                //    CityCode = string.Empty,
                                                //    StateCode = jDetail.StateCode
                                                //};
                                                //bjpProcessor.InsertCity(ref cModel);
                                                //if (!string.IsNullOrWhiteSpace(cModel.CityCode))
                                                //{
                                                //    jDetail.GetType().GetProperty(col.ColumnName).SetValue(jDetail, cModel.CityCode);
                                                //}
                                                //else
                                                //{
                                                row[additionalColumns[2]] = "Failed";
                                                row[additionalColumns[3]] += "<li>City Not Found In Our Record</li>";
                                                //}
                                            }
                                            break;
                                        case "SPOCEmail":
                                            string email = Convert.ToString(row[col.ColumnName]);
                                            if (!string.IsNullOrWhiteSpace(email) && email!=Constants.NotAvailalbe)
                                            {
                                                if (!emailHandler.IsValidEmail(email))
                                                {
                                                    row[additionalColumns[2]] = "Failed";
                                                    row[additionalColumns[3]] += $"<li>SPOC email is not in valid format, please use {Constants.NotAvailalbe} if SPOC email is not available.</li>";
                                                }
                                            }
                                            break;
                                        case "CompanyName":
                                            string companyName = Convert.ToString(row[col.ColumnName]);
                                            if (!authProcessor.CheckIfEmployerExists(companyName,true))
                                            {
                                                string mailId = companyName.ToLower().Replace(" ", "_");
                                                var u = new UserModel
                                                {
                                                    CompanyName = companyName,
                                                    Email = mailId,
                                                    Password = RandomGenerator.GetRandom(),
                                                    RoleId = 3,
                                                    ProfilePic = string.Empty
                                                };
                                                bool isRegister = authProcessor.RegisterEmployer(u,true);
                                            }
                                            t =
                                            bjpProcessor.GetIdFromValue(companyName, col.ColumnName);
                                            if (null != t && t.Rows.Count > 0)
                                            {
                                                jDetail.Userid = Convert.ToInt32(t.Rows[0]["Id"]);
                                            }
                                            else
                                            {
                                                row[additionalColumns[2]] = "Failed";
                                                row[additionalColumns[3]] += "<li>User Not Found In Our Record</li>";
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if ("Failed" != Convert.ToString(row[additionalColumns[2]]))
                                {
                                    jDetail.IsFromBulkUpload = true;
                                    jDetail.CreatedBy = Convert.ToString(user.UserId);
                                    //ValidateJobModel(ref jDetail);
                                    if (jobPostProcessor.AddJobPostData(jDetail))
                                    {
                                        row[additionalColumns[2]] = "Success";
                                    }
                                    else
                                    {
                                        row[additionalColumns[2]] = "Failed";
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                row[additionalColumns[2]] = "Failed";
                                row[additionalColumns[3]] += $"<li>{ex.Message}</li>";
                            }
                            sData = AddSummary(row);
                            fileSummary.Summary.Add(sData);
                            SaveDetailToAudit(sData, fileSummary.FileName, user.UserId);
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
                    string.Format("{0}", "JobPostMapping.xml file is not in proper format to convert in data table."));
            }
        }

        private void SimpliFyTableDataForMultipleCitiesForSingleJob(DataTable table)
        {
            int rowCount = table.Rows.Count;
            int rowCounter = 0;

            // Will iterate from 1 bcoz 0 index will be for header
            for (int i = 1; i < rowCount; i++)
            {
                rowCounter++;
                if (rowCounter <= rowCount)
                {
                    DataRow row = table.Rows[i];
                    string[] cities = Convert.ToString(row["CityCode"]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (cities != null && cities.Length > 1)
                    {
                        int index = -1;
                        foreach (string city in cities)
                        {
                            index++;
                            if (index == 0)
                            {
                                row["CityCode"] = city;
                            }
                            else
                            {
                                DataRow r = table.NewRow();
                                foreach (DataColumn col in table.Columns)
                                {
                                    r[col.ColumnName] = row[col.ColumnName];
                                }
                                r["CityCode"] = city;
                                table.Rows.Add(r);
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void ValidateQuarters(DataRow row, string[] additionalColumns)
        {
            if (string.IsNullOrEmpty(Convert.ToString(row["Quarter1"]))
                && string.IsNullOrEmpty(Convert.ToString(row["Quarter2"]))
                && string.IsNullOrEmpty(Convert.ToString(row["Quarter3"]))
                && string.IsNullOrEmpty(Convert.ToString(row["Quarter4"])))
            {
                row[additionalColumns[2]] = "Failed";
                row[additionalColumns[3]] += $"<li>At least one quarter must have a value</li>";
            }
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(row["Quarter1"]))
                    && Convert.ToInt32(row["Quarter1"]) < 1
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter2"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter3"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter4"]))
                    )
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Q1 must not be less than one</li>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(row["Quarter2"]))
                    && Convert.ToInt32(row["Quarter2"]) < 1
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter1"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter3"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter4"]))
                    )
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Q2 must not be less than one</li>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(row["Quarter3"]))
                    && Convert.ToInt32(row["Quarter3"]) < 1
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter1"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter2"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter4"]))
                    )
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Q3 must not be less than one</li>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(row["Quarter4"]))
                    && Convert.ToInt32(row["Quarter4"]) < 1
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter1"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter2"]))
                    && string.IsNullOrEmpty(Convert.ToString(row["Quarter3"]))
                    )
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Q4 must not be less than one</li>";
                }
            }
        }

        private void ValidateJobExpiryDates(DataRow row, string[] additionalColumns)
        {
            string posEndDate = Convert.ToString(row["PositionEndDate"]);
            string posStartDate = Convert.ToString(row["PositionStartDate"]);
            bool compareDateRange = true;
            if (!string.IsNullOrWhiteSpace(posStartDate))
            {
                try
                {
                    var dt = Convert.ToDateTime(posStartDate);
                    row["PositionStartDate"] = dt;
                }
                catch (Exception ex)
                {
                    compareDateRange = false;
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Posting date is not in correct format</li>";
                }
            }

            if (!string.IsNullOrWhiteSpace(posEndDate))
            {
                try
                {
                    var dt = Convert.ToDateTime(posEndDate);
                    row["PositionEndDate"] = dt;
                }
                catch (Exception ex)
                {
                    compareDateRange = false;
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Expiry date is not in correct format</li>";
                }
            }
            if (compareDateRange)
            {
                if (Convert.ToDateTime(row["PositionEndDate"]) < Convert.ToDateTime(row["PositionStartDate"]))
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Job expiry date should not be less than position start date.</li>";
                }
            }
        }

        private void ValidateSpocContact(DataRow row, string[] additionalColumns)
        {
            string spocContact = Convert.ToString(row["SPOCContact"]);
            if (!string.IsNullOrWhiteSpace(spocContact) && spocContact!=Constants.NotAvailalbe)
            {
                if (spocContact.Substring(0, 3) != "+91")
                {
                    spocContact = "+91" + spocContact;
                }
                if (Regex.IsMatch(spocContact, @"^(\+91)\d{10}"))
                {
                    row["SPOCContact"] = spocContact;
                }
                else
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>SPOC Contact is not in correct format - Enter 10 digit mobile number or {Constants.NotAvailalbe}</li>";

                }
            }       
        }

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

        private void GetJobTitle(DataRow row, string[] additionalColumns)
        {
            string colName = "JobTitleId";
            string[] jobTitles = new string[]
            {
               Convert.ToString(row["JobTitleByEmployer"]).Trim(),
                //Convert.ToString(row["JobRole2"]).Trim(),
                //Convert.ToString(row["JobRole3"]).Trim()
            };
            if (!row.Table.Columns.Contains(colName))
            {
                row.Table.Columns.Add(colName);
            }
            int i = 0;
            int[] ids = new int[jobTitles.Length];
            foreach (string title in jobTitles)
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var t =
                           bjpProcessor.GetIdFromValue(title, colName);
                    if (null != t && t.Rows.Count > 0)
                    {
                        ids[i++] = Convert.ToInt32(t.Rows[0]["ID"]);
                    }
                    else
                    {
                        row[additionalColumns[2]] = "Failed";
                        row[additionalColumns[3]] += $"<li>{title} Job Title Not Found In Our Record</li>";
                    }
                }
            }
            row[colName] = string.Join(Constants.CommaSeparator, ids.Where(id => id > 0));
        }

        private void ManageJobType(DataRow row, string[] additionalColumns)
        {
            string cName = "JobType";
            //  Job type will be only from Fresher, Experience or Any.
            string jType = Convert.ToString(row[cName]);

            if (!string.IsNullOrWhiteSpace(jType))
            {
                if(jType!= "Fresher" && jType!= "Experience" && jType!= "Any")
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>Job Type {jType} is not valid,Enter Fresher/Experience/Any as correct Job Type.</li>";
                    return;
                }
            }

            switch (jType)
            {
                case "Fresher":
                    row["MinExp"] = 0;
                    row["MaxExp"] = 0;
                    break;
                case "Experience":
                    if (string.IsNullOrWhiteSpace(Convert.ToString(row["MinExp"])))
                    {
                        row[additionalColumns[2]] = "Failed";
                        row[additionalColumns[3]] += $"<li>Min experience is required if job type is experienced.</li>";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(row["MinExp"])) && !string.IsNullOrWhiteSpace(Convert.ToString(row["MaxExp"])))
                        {
                            if (Convert.ToInt32(row["MaxExp"])>0 && Convert.ToInt32(row["MinExp"]) > Convert.ToInt32(row["MaxExp"]))
                            {
                                row[additionalColumns[2]] = "Failed";
                                row[additionalColumns[3]] += $"<li>Min experience must be less than Max experience.</li>";
                            }
                        }
                    }
                    break;
                case "Any":
                    row["MinExp"] = -1;
                    row["MaxExp"] = -1;
                    break;
            }

            var t =
                   bjpProcessor.GetIdFromValue(jType, cName);
            if (null != t && t.Rows.Count > 0)
            {
                row[cName] = Convert.ToInt32(t.Rows[0]["Id"]);
            }
        }

        private bool IsAllowToPostJob(UserViewModel user, DataRow row)
        {
            string cName = Convert.ToString(row["CompanyName"]);
            if (string.IsNullOrWhiteSpace(cName) || user.RoleName == Constants.AdminRole || user.CompanyName.ToUpper() == cName.ToUpper())
            {
                return true;
            }
            return false;
        }

        private void ResolveJobData(out JobPostModel job, DataRow row, string[] additionalColumns)
        {
            job = new JobPostModel();
            var pInfos = job.GetType().GetProperties();

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
                                string val = Convert.ToString(row[pInfo.Name]);
                                if (!string.IsNullOrWhiteSpace(val))
                                {
                                    pInfo.SetValue(job, val);
                                }
                                break;
                            case "Int32":
                                int inVal = Convert.ToInt32(row[pInfo.Name]);
                                if (inVal > 0)
                                {
                                    pInfo.SetValue(job, inVal);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    row[additionalColumns[2]] = "Failed";
                    row[additionalColumns[3]] += $"<li>{pInfo.Name} is not in valid format</li>";
                }
            }
        }

        private BulkJobPostSummaryDetailViewModel AddSummary(DataRow row)
        {
            var summary = new BulkJobPostSummaryDetailViewModel();
            var pInfos = summary.GetType().GetProperties();

            foreach (var pInfo in pInfos)
            {
                pInfo.SetValue(summary, Convert.ToString(row[pInfo.Name]));
            }
            return summary;
        }

        private void ValidateJobModel(ref JobPostModel job)
        {
            var pInfos = job.GetType().GetProperties();
            foreach (var pInfo in pInfos)
            {
                var val = pInfo.GetValue(job);
                if (null == val)
                {
                    DataTable t = null;
                    switch (pInfo.Name)
                    {
                        case "CountryCode":
                            t =
                                bjpProcessor.GetIdFromValue(Convert.ToString("INDIA"), pInfo.Name);
                            if (null != t && t.Rows.Count > 0)
                            {
                                pInfo.SetValue(job, t.Rows[0]["Id"]);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void SaveDetailToAudit(BulkJobPostSummaryDetailViewModel detail, string fileName, int userId)
        {
            try
            {
                var dModel = new BulkJobPostSummaryDetail
                {
                    CompanyName = detail.CompanyName,
                    CreatedBy = userId,
                    CTC = detail.CTC,
                    ErrorDetails = detail.ErrorDetails,
                    FileName = fileName,
                    //FinancialYear = detail.FinancialYear,
                    HiringCriteria = detail.HiringCriteria,
                    JobDetails = detail.JobDetails,
                    JobLocation = detail.CityCode,
                    //JobRole1 = detail.JobRole1,
                    //JobRole2 = detail.JobRole2,
                    //JobRole3 = detail.JobRole3,
                    JobTitle = detail.JobTitleByEmployer,
                    JobType = detail.JobType,
                    MaxExp = detail.MaxExp,
                    MinExp = detail.MinExp,
                    ProcessedBy = detail.ProcessedBy,
                    ProcessedOn = detail.ProcessedOn,
                    //Quarter1 = detail.Quarter1,
                    //Quarter2 = detail.Quarter2,
                    //Quarter3 = detail.Quarter3,
                    //Quarter4 = detail.Quarter4,
                    SerialNo = detail.SequenceNo,
                    SPOC = detail.SPOC,
                    SPOCContact = detail.SPOCContact,
                    SPOCEmail = detail.SPOCEmail,
                    State = detail.StateCode,
                    Status = detail.Status,
                    Total = detail.Total
                };
                bjpProcessor.SaveDetailToAudit(dModel);
            }
            catch (Exception ex)
            {
                string errMessage = $"<li>Unable to insert job summary due to:   {ex.Message}.</li>";
                detail.ErrorDetails += errMessage;
            }
        }
    }
}
