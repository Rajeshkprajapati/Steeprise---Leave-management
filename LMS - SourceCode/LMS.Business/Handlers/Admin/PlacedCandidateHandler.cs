using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Admin;
using LMS.Data.DataModel.Admin.PlacedCandidate;
using LMS.Data.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.PlacedCandidate;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.FilesUtility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LMS.Business.Handlers.Admin
{
    public class PlacedCandidateHandler : IPlacedCandidateHandler
    {
        private readonly IPlacedCandidateRepository _placedCandidateRepository;
        private readonly IHostingEnvironment environment;
        public PlacedCandidateHandler(IConfiguration configuration, IHostingEnvironment _environment)
        {
            var factory = new ProcessorFactoryResolver<IPlacedCandidateRepository>(configuration);
            _placedCandidateRepository = factory.CreateProcessor();
            environment = _environment;
        }

        public bool UploadFile(UserViewModel user, List<IFormFile> files)
        {
            string roleName = user.RoleName;
            //var status = true;
            string mappingFilePath =
                Path.Combine(environment.WebRootPath, "DataMappings", "PlacedCandidate", "PlacedCandidateMapping.xml");
            DataTable dt = XmlProcessor.XmlToTable(mappingFilePath);
            if (null != dt)
            {
                foreach (var file in files)
                {
                    DataTable dTable = null;
                    NPOIManager.ReadFile(file, dt, out dTable, true, roleName);
                    if (null != dTable && dTable.Rows.Count > 1)
                    {
                        //string[] additionalColumns = new string[] { "CreatedDate", "CreatedBy" };
                        //ExtendTable(additionalColumns, ref dTable, user.UserId, true);
                        //PlacedCandidateModel placeduser = null;
                        int header = -1;
                        foreach (DataRow row in dTable.Rows)
                        {
                            header++;
                            if (header == 0)
                            {
                                continue;
                            }
                            PlacedCandidateModel placeduser = new PlacedCandidateModel()
                            {
                                AverageofNoOfMonthsofPlacement = row["AverageofNoOfMonthsofPlacement"] as string ?? "",
                                AvgofNoofdaysbetweennDOCDOP = row["AvgofNoofdaysbetweennDOCDOP"] as string ?? "",
                                CandidateEmail = row["CandidateEmail"] as string ?? "",
                                CandidateID = row["CandidateID"] as string ?? "",
                                CandidateName = row["CandidateName"] as string ?? "",
                                Castecategory = row["Castecategory"] as string ?? "",
                                CertificateDate = row["CertificateDate"] as string ?? "",
                                Certified = row["Certified"] as string ?? "",
                                CountofPartnerID = row["CountofPartnerID"] as string ?? "",
                                CountofSCTrainingCentreID = row["CountofSCTrainingCentreID"] as string ?? "",
                                EducationAttained = row["EducationAttained"] as string ?? "",
                                EmployerspocEmail = row["EmployerspocEmail"] as string ?? "",
                                EmployerspocMobile = row["EmployerspocMobile"] as string ?? "",
                                EmployerSpocName = row["EmployerSpocName"] as string ?? "",
                                EmployerType = row["EmployerType"] as string ?? "",
                                FirstEmploymentCreatedDate = row["FirstEmploymentCreatedDate"] as string ?? "",
                                FromDate = row["FromDate"] as string ?? "",
                                FYWise = row["FYWise"] as string ?? "",
                                Gender = row["Gender"] as string ?? "",
                                Jobrole = row["Jobrole"] as string ?? "",
                                OrganisationDistrict = row["OrganisationDistrict"] as string ?? "",
                                OrganisationState = row["OrganisationState"] as string ?? "",
                                OrganizationAddress = row["OrganizationAddress"] as string ?? "",
                                OrganizationName = row["OrganizationName"] as string ?? "",
                                PartnerName = row["PartnerName"] as string ?? "",
                                PartnerSPOCEmail = row["PartnerSPOCEmail"] as string ?? "",
                                PartnerSPOCMobile = row["PartnerSPOCMobile"] as string ?? "",
                                PartnerSPOCName = row["PartnerSPOCName"] as string ?? "",
                                SectorName = row["SectorName"] as string ?? "",
                                SelfEmployedDistrict = row["SelfEmployedDistrict"] as string ?? "",
                                SelfEmployedState = row["SelfEmployedState"] as string ?? "",
                                SumofCandidateContactNo = row["AverageofNoOfMonthsofPlacement"] as string ?? "",
                                SumofSalleryPerMonth = row["SumofSalleryPerMonth"] as string ?? "",
                                SumofTCSPOCMobile = row["SumofTCSPOCMobile"] as string ?? "",
                                TCDistrict = row["TCDistrict"] as string ?? "",
                                TCSPOCEmail = row["TCSPOCEmail"] as string ?? "",
                                TCSPOCName = row["TCSPOCName"] as string ?? "",
                                TCState = row["TCState"] as string ?? "",
                                ToDate = row["ToDate"] as string ?? "",
                                TrainingCentreName = row["TrainingCentreName"] as string ?? "",
                                TrainingType = row["TrainingType"] as string ?? "",                                
                            };
                            try
                            {
                                _placedCandidateRepository.UploadFileData(placeduser, user.UserId);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public IList<PlacedCandidateViewModel> GetAllCandidate()
        {
            var dt = _placedCandidateRepository.GetAllCandidate();
            IList<PlacedCandidateViewModel> candidate = new List<PlacedCandidateViewModel>();
            foreach (DataRow row in dt.Rows)
            {
                PlacedCandidateViewModel user = new PlacedCandidateViewModel() {
                    AverageofNoOfMonthsofPlacement = row["AverageofNoOfMonthsofPlacement"] as string ?? "",
                    AvgofNoofdaysbetweennDOCDOP = row["AvgofNoofdaysbetweennDOCDOP"] as string ?? "",
                    CandidateEmail = row["CandidateEmail"] as string ?? "",
                    CandidateID = row["CandidateID"] as string ?? "",
                    CandidateName = row["CandidateName"] as string ?? "",
                    Castecategory = row["Castecategory"] as string ?? "",
                    CertificateDate = row["CertificateDate"] as string ?? "",
                    Certified = row["Certified"] as string ?? "",
                    CountofPartnerID = row["CountofPartnerID"] as string ?? "",
                    CountofSCTrainingCentreID = row["CountofSCTrainingCentreID"] as string ?? "",
                    EducationAttained = row["EducationAttained"] as string ?? "",
                    EmployerspocEmail = row["EmployerspocEmail"] as string ?? "",
                    EmployerspocMobile = row["EmployerspocMobile"] as string ?? "",
                    EmployerSpocName = row["EmployerSpocName"] as string ?? "",
                    EmployerType = row["EmployerType"] as string ?? "",
                    FirstEmploymentCreatedDate = row["FirstEmploymentCreatedDate"] as string ?? "",
                    FromDate = row["FromDate"] as string ?? "",
                    FYWise = row["FYWise"] as string ?? "",
                    Gender = row["Gender"] as string ?? "",
                    Jobrole = row["Jobrole"] as string ?? "",
                    OrganisationDistrict = row["OrganisationDistrict"] as string ?? "",
                    OrganisationState = row["OrganisationState"] as string ?? "",
                    OrganizationAddress = row["OrganizationAddress"] as string ?? "",
                    OrganizationName = row["OrganizationName"] as string ?? "",
                    PartnerName = row["PartnerName"] as string ?? "",
                    PartnerSPOCEmail = row["PartnerSPOCEmail"] as string ?? "",
                    PartnerSPOCMobile = row["PartnerSPOCMobile"] as string ?? "",
                    PartnerSPOCName = row["PartnerSPOCName"] as string ?? "",
                    SectorName = row["SectorName"] as string ?? "",
                    SelfEmployedDistrict = row["SelfEmployedDistrict"] as string ?? "",
                    SelfEmployedState = row["SelfEmployedState"] as string ?? "",
                    SumofCandidateContactNo = row["SumofCandidateContactNo"] as string ?? "",
                    SumofSalleryPerMonth = row["SumofSalleryPerMonth"] as string ?? "",
                    SumofTCSPOCMobile = row["SumofTCSPOCMobile"] as string ?? "",
                    TCDistrict = row["TCDistrict"] as string ?? "",
                    TCSPOCEmail = row["TCSPOCEmail"] as string ?? "",
                    TCSPOCName = row["TCSPOCName"] as string ?? "",
                    TCState = row["TCState"] as string ?? "",
                    ToDate = row["ToDate"] as string ?? "",
                    TrainingCentreName = row["TrainingCentreName"] as string ?? "",
                    TrainingType = row["TrainingType"] as string ?? "",
                    CreatedDate = row["CreatedDate"] as DateTime? ?? DateTime.Now,
                };
                candidate.Add(user);
            }
            return candidate;
        }

        public DataTable GetDataInExcel()
        {
            var dt = _placedCandidateRepository.GetAllCandidate();
            DataTable candidate = new DataTable();
            candidate.TableName = "Export Data";
            candidate.Columns.Add("CandidateID", typeof(string));
            candidate.Columns.Add("CandidateName", typeof(string));
            candidate.Columns.Add("CandidateEmail", typeof(string));
            candidate.Columns.Add("Castecategory", typeof(string));
            candidate.Columns.Add("EducationAttained", typeof(string));
            candidate.Columns.Add("EmployerspocEmail", typeof(string));
            candidate.Columns.Add("CreatedDate", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
               candidate.Rows.Add(
                     Convert.ToString(row["CandidateID"] as string ?? ""),
                    Convert.ToString(row["CandidateName"] as string ?? ""),
                    Convert.ToString(row["CandidateEmail"] as string ?? ""),
                    Convert.ToString(row["Castecategory"] as string ?? ""),
                    Convert.ToString(row["EducationAttained"] as string ?? ""),
                    Convert.ToString(row["EmployerspocEmail"] as string ?? ""),
                    Convert.ToString(row["CreatedDate"] as DateTime? ?? DateTime.Now)
                    );
                
                candidate.AcceptChanges();
            }
            return candidate;
        }
    }
}
