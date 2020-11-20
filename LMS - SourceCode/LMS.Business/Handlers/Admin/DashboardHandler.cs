using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Admin;
using LMS.Business.Shared;
using LMS.Data.DataModel.Admin.Dashboard;
using LMS.Data.Interfaces.Admin;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Admin.Dashboard;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.FilesUtility;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LMS.Business.Handlers.Admin
{
    public class DashboardHandler : IDashboardHandler
    {
        private readonly IDashboardRepository dashboard;
        private readonly IMasterDataRepository masterRepository;

        public DashboardHandler(IConfiguration configuration)
        {
            var factory = new ProcessorFactoryResolver<IDashboardRepository>(configuration);
            dashboard = factory.CreateProcessor();
            var masterFactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterRepository = masterFactory.CreateProcessor();
        }

        public IList<DemandAggregationDataOnQuarterViewModel> GetDemandAggregationDataOnQuarter(int userId, DemandAggregationSearchItems search)
        {
            var dataOnQuarters = new List<DemandAggregationDataOnQuarterViewModel>();
            var filters = new DemandAggregationSearchFilters
            {
                Employers = search.Employers,
                FinancialYear = search.FinancialYear,
                JobRoles = search.JobRoles,
                JobStates = search.JobStates,
                UserId = userId,
                UserRole = search.UserRole
            };
            var data = dashboard.GetDemandAggregationDataOnQuarter(filters);
            foreach (DataRow row in data.Rows)
            {
                dataOnQuarters.Add(new DemandAggregationDataOnQuarterViewModel
                {
                    Year = Convert.ToInt32(row["PostedYear"]),
                    //Month = Convert.ToInt32(row["PostedMonth"]),
                    DemandAggregations = SimplifyTotalDemandAggregatedData(row)
                });
            }
            return dataOnQuarters;
        }

        public IList<DemandAggregationOnJobRolesViewModel> GetDemandAggregationDataOnJobRole(int userId, DemandAggregationSearchItems search)
        {
            var dataOnJobRole = new List<DemandAggregationOnJobRolesViewModel>();
            var filters = new DemandAggregationSearchFilters
            {
                Employers = search.Employers,
                FinancialYear = search.FinancialYear,
                JobRoles = search.JobRoles,
                JobStates = search.JobStates,
                UserId = userId,
                UserRole = search.UserRole
            };
            var data = dashboard.GetDemandAggregationDataOnJobRole(filters);
            foreach (DataRow row in data.Rows)
            {
                dataOnJobRole.Add(new DemandAggregationOnJobRolesViewModel
                {
                    JobRoleId = Convert.ToInt32(row["JobTitleId"]),
                    JobRole = Convert.ToString(row["JobTitleName"]),
                    Year = Convert.ToInt32(row["PostedYear"]),
                    //Month = Convert.ToInt32(row["PostedMonth"]),
                    DemandAggregations = SimplifyTotalDemandAggregatedData(row)
                });
            }
            return dataOnJobRole;
        }

        public IList<DemandAggregationOnStatesViewModel> GetDemandAggregationOnState(int userId, DemandAggregationSearchItems search)
        {
            var dataOnState = new List<DemandAggregationOnStatesViewModel>();
            var filters = new DemandAggregationSearchFilters
            {
                Employers = search.Employers,
                FinancialYear = search.FinancialYear,
                JobRoles = search.JobRoles,
                JobStates = search.JobStates,
                UserId = userId,
                UserRole = search.UserRole
            };
            var data = dashboard.GetDemandAggregationOnState(filters);
            foreach (DataRow row in data.Rows)
            {
                dataOnState.Add(new DemandAggregationOnStatesViewModel
                {
                    StateCode = Convert.ToString(row["StateCode"]),
                    State = Convert.ToString(row["StateName"]),
                    //Month = Convert.ToInt32(row["PostedMonth"]),
                    Year = Convert.ToInt32(row["PostedYear"]),
                    DemandAggregations = SimplifyTotalDemandAggregatedData(row)
                });
            }
            return dataOnState;
        }

        public IList<DemandAggregationOnEmployersViewModel> GetDemandAggregationDataOnEmployer(int userId, DemandAggregationSearchItems search)
        {
            var dataOnEmployer = new List<DemandAggregationOnEmployersViewModel>();
            var filters = new DemandAggregationSearchFilters
            {
                Employers = search.Employers,
                FinancialYear = search.FinancialYear,
                JobRoles = search.JobRoles,
                JobStates = search.JobStates,
                UserId = userId,
                UserRole = search.UserRole
            };
            var data = dashboard.GetDemandAggregationOnEmployer(filters);
            foreach (DataRow row in data.Rows)
            {
                dataOnEmployer.Add(new DemandAggregationOnEmployersViewModel
                {
                    //EmployerId = Convert.ToInt32(row["UserId"]),
                    //EmployerFName = Convert.ToString(row["FirstName"]),
                    // EmployerLName = Convert.ToString(row["LastName"]),
                    Company = Convert.ToString(row["CompanyName"]),
                    //Month = Convert.ToInt32(row["PostedMonth"]),
                    Year = Convert.ToInt32(row["PostedYear"]),
                    DemandAggregations = SimplifyTotalDemandAggregatedData(row)
                });
            }
            return dataOnEmployer;
        }

        public IList<DemandAggregationDetailsViewModel> ViewDemandAggregationDetails(string onBasis, string value, DemandAggregationSearchItems search)
        {
            var filters = new DemandAggregationSearchFilters
            {
                Employers = search.Employers,
                FinancialYear = search.FinancialYear,
                JobRoles = search.JobRoles,
                JobStates = search.JobStates,
                UserRole = search.UserRole
            };
            var data = dashboard.ViewDemandAggregationDetails(onBasis,value, filters);
            return SimplifyDemandAggregationDetails(data);
        }

        public IWorkbook GetDemandAggregationReportData(DemandAggregationSearchItems search, FileExtensions fileExtension)
        {
            var filters = new DemandAggregationSearchFilters
            {
                Employers = search.Employers,
                FinancialYear = search.FinancialYear,
                JobRoles = search.JobRoles,
                JobStates = search.JobStates,
                UserRole = search.UserRole
            };
            var reportData = dashboard.GetDemandAggregationReportData(filters);
            return ModifyWorkBook(NPOIManager.CreateExcelWorkBook(reportData, fileExtension, "Demand Aggregation Report"));
        }

        private IWorkbook ModifyWorkBook(IWorkbook wBook)
        {
            var sheet = wBook.GetSheetAt(0);
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    var cell = row.GetCell(j);
                    switch (sheet.GetRow(0).GetCell(j).ToString().Trim())
                    {
                        case "Q1":
                        case "Q2":
                        case "Q3":
                        case "Q4":
                        case "MinExperience":
                        case "MaxExperience":
                            string val = Convert.ToString(cell.ToString());
                            cell.SetCellType(CellType.Numeric);
                            cell.SetCellValue(Convert.ToDouble(val));
                            break;
                        case "Total":
                            cell.SetCellType(CellType.Formula);
                            cell.SetCellFormula($"SUM(Q{i+1}:T{i+1})");
                            break;
                        default:
                            break;
                    }
                }
            }
            return wBook;
        }

        public IList<JobTitleViewModel> GetJobTitles()
        {
            DataTable dt = masterRepository.GetJobRoles();
            var jobTitlesList = new List<JobTitleViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jobTitlesList.Add(new JobTitleViewModel()
                {
                    JobTitleId = Convert.ToInt32(dt.Rows[i]["JobTitleId"]),
                    JobTitleName = Convert.ToString(dt.Rows[i]["JobTitleName"]),
                });
            }
            return jobTitlesList;
        }

        public IList<StateViewModel> GetStates(string country)
        {
            DataTable dt = masterRepository.GetStates(country);
            var states = new List<StateViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                states.Add(new StateViewModel()
                {
                    State = Convert.ToString(dt.Rows[i]["State"]),
                    StateCode = Convert.ToString(dt.Rows[i]["StateCode"]),
                    CountryCode = country,
                });
            }
            return states;
        }

        public IList<UserViewModel> GetEmployers(bool isAll)
        {
            DataTable dt = masterRepository.GetEmployers(null,isAll);
            var employers = new List<UserViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var emp = new UserViewModel()
                {
                    UserId = Convert.ToInt32(dt.Rows[i]["Userid"]),
                    FirstName = Convert.ToString(dt.Rows[i]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[i]["LastName"]),
                    CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                    CityName = Convert.ToString(dt.Rows[i]["City"])

                };
                if (!employers.Any(e => e.CompanyName == emp.CompanyName))
                {
                    employers.Add(emp);
                }
            }
            return employers;
        }

        private DemandAggregationDataViewModel SimplifyTotalDemandAggregatedData(DataRow row)
        {
            var d = new DemandAggregationDataViewModel();
            if (!Convert.IsDBNull(row["Q1"]))
            {
                d.Q1 = Convert.ToInt32(row["Q1"]);
            }
            if (!Convert.IsDBNull(row["Q2"]))
            {
                d.Q2 = Convert.ToInt32(row["Q2"]);
            }
            if (!Convert.IsDBNull(row["Q3"]))
            {
                d.Q3 = Convert.ToInt32(row["Q3"]);
            }
            if (!Convert.IsDBNull(row["Q4"]))
            {
                d.Q4 = Convert.ToInt32(row["Q4"]);
            }

            return d;
        }

        private IList<DemandAggregationDetailsViewModel> SimplifyDemandAggregationDetails(DataTable table)
        {
            IList<DemandAggregationDetailsViewModel> lstDetails = new List<DemandAggregationDetailsViewModel>();
            foreach (DataRow row in table.Rows)
            {
                int fYear = Convert.ToInt32(row["FinancialYear"]);
                string _fYear = $"{fYear}-{fYear + 1}";
                lstDetails.Add(new DemandAggregationDetailsViewModel
                {
                    City = Convert.ToString(row["City"]),
                    Company = Convert.ToString(row["CompanyName"]),
                    Country = Convert.ToString(row["Country"]),
                    CreatedByFName = Convert.ToString(row["CreatedByFirstName"]),
                    CreatedByLName = Convert.ToString(row["CreatedByLastName"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    FinancialYear = _fYear,
                    JobRole = Convert.ToString(row["JobTitleName"]),
                    State = Convert.ToString(row["State"]),
                    Quarter1 = Convert.ToInt32(row["Quarter1"]),
                    Quarter2 = Convert.ToInt32(row["Quarter2"]),
                    Quarter3 = Convert.ToInt32(row["Quarter3"]),
                    Quarter4 = Convert.ToInt32(row["Quarter4"])
                });
            }
            return lstDetails;
        }

        public List<CityViewModel> GetCityList(string StateCode)
        {
            DataTable city = masterRepository.GetCities(StateCode);
            if (city.Rows.Count > 0)
            {
                List<CityViewModel> lstCity = new List<CityViewModel>();
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
                return lstCity;
            }
            throw new DataNotFound("Cities not found, please contact your tech deck.");
        }
    }
}
