using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Data.Interfaces.Jobseeker;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;

namespace LMS.Business.Handlers.Jobseeker
{
    public class ResumeBuilderHandler : IResumeBuilderHandler
    {
        private readonly IHostingEnvironment environment;
        private readonly IResumeBuilderRepository resumeBuilderRepository;
        private readonly IMasterDataRepository masterDataRepository;
        public ResumeBuilderHandler(IHostingEnvironment env, IConfiguration configuration)
        {
            environment = env;
            var factory = new ProcessorFactoryResolver<IResumeBuilderRepository>(configuration);
            resumeBuilderRepository = factory.CreateProcessor();
            var mfactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterDataRepository = mfactory.CreateProcessor();
        }

        public void CreateResume(int userId, string htmlContent)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("<!DOCTYPE html><html><head><meta charset = 'utf-8'/><title></title></head><body>");
            sBuilder.Append(htmlContent);
            sBuilder.Append("</body></html>");

            string tempResumeFile = string.Format("{0}\\{1}", "\\Resume", string.Format("_{0}{1}", DateTime.Now.Ticks, ".html"));
            string destinationFile = environment.WebRootPath + tempResumeFile;

            using (FileStream fStream = new FileStream(destinationFile, FileMode.Create))
            {
                using (StreamWriter sWriter = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sWriter.WriteLine(sBuilder.ToString());
                }
            }

            bool processed = false;
            while (!processed)
            {
                if (File.Exists(destinationFile))
                {
                    using (Document doc = new Document(destinationFile, FileFormat.Html, Spire.Doc.Documents.XHTMLValidationType.None))
                    {
                        string resumeFile = string.Format("{0}\\{1}", "\\Resume", string.Format("_{0}{1}", DateTime.Now.Ticks, ".docx"));
                        string rFile = environment.WebRootPath + resumeFile;
                        MemoryStream mStream = new MemoryStream();
                        doc.SaveToStream(mStream,
                            FileFormat.Docx);
                        mStream.Position = 0;
                        using (FileStream _fs = new FileStream(rFile, FileMode.Create))
                        {
                            mStream.CopyTo(_fs);
                        }

                        var oldResumePath = GetResume(userId);
                        if (File.Exists(oldResumePath))
                        {
                            File.Delete(oldResumePath);
                        }

                        UpdateResumePath(userId, resumeFile);

                        if (File.Exists(destinationFile))
                        {
                            File.Delete(destinationFile);
                        }
                        processed = true;
                    }

                }
            }
        }

        public string GetResume(int userId)
        {
            var fName = resumeBuilderRepository.GetResume(userId);
            var fPath = environment.WebRootPath + fName;
            //var fPath = Path.Combine(environment.WebRootPath, fName);
            return fPath;
        }

        private bool UpdateResumePath(int userId, string resumePath)
        {
            return resumeBuilderRepository.UpdateResumePath(userId, resumePath);
        }

        public ResumeViewModel GetUserInfoToCreateResume(int userId)
        {
            var dSet = resumeBuilderRepository.GetUserDetailsForResumeBuilder(userId);
            var model = new ResumeViewModel
            {
                EducationalDetails = new List<EducationalDetails>(),
                ExperienceDetails = new List<ExperienceDetails>(),
                PersonalDetails = new UserViewModel(),
                Skills = new Skills()
            };
            if (null != dSet && dSet.Tables.Count > 0)
            {
                int tIndex = -1;
                foreach (DataTable table in dSet.Tables)
                {
                    tIndex++;
                    switch (tIndex)
                    {
                        case 0:
                            model.ExperienceDetails = JsonConvert.DeserializeObject<ExperienceDetails[]>(Convert.ToString(table.Rows[0]["ExperienceDetails"]));
                            model.EducationalDetails = JsonConvert.DeserializeObject<EducationalDetails[]>(Convert.ToString(table.Rows[0]["EducationalDetails"]));
                            model.Skills = JsonConvert.DeserializeObject<Skills>(Convert.ToString(table.Rows[0]["Skills"]));
                            SerializeProfessionalDetails(model.ExperienceDetails);
                            SerializeEducationalDetails(model.EducationalDetails);
                            break;
                        case 1:
                            model.PersonalDetails = SerializeUserPersonalDetails(table.Rows[0]);
                            break;
                        default:
                            break;
                    }
                }
            }
            return model;
        }

        public bool InsertExperienceDetails(int userId, ExperienceDetails[] exp, Skills skills)
        {
            var experienceModel = new Data.DataModel.JobSeeker.ExperienceDetails[exp.Length];
            int i = 0;
            while (i < exp.Length)
            {
                experienceModel[i] = new Data.DataModel.JobSeeker.ExperienceDetails
                {
                    Id = exp[i].Id,
                    AnnualSalary = exp[i].AnnualSalary,
                    Designation = exp[i].Designation,
                    Industry = exp[i].Industry,
                    IsCurrentOrganization = exp[i].IsCurrentOrganization,
                    JobProfile = exp[i].JobProfile,
                    NoticePeriod = exp[i].NoticePeriod,
                    Organization = exp[i].Organization,
                    ServingNoticePeriod = exp[i].ServingNoticePeriod,
                    WorkingFrom = exp[i].WorkingFrom,
                    WorkingTill = exp[i].WorkingTill,
                    WorkLocation = exp[i].WorkLocation,
                    Skills = new Data.DataModel.JobSeeker.Skills
                    {
                        SkillSets = exp[i].Skills.SkillSets
                    }
                };
                i++;
            }
            foreach (Data.DataModel.JobSeeker.ExperienceDetails item in experienceModel)
            {
                if(item != null && item.Id <= 0)
                {
                    item.Id = experienceModel.Where(e => e.Id > 0).ToList().Count + 1;
                }
            }

            var skillsModel = new Data.DataModel.JobSeeker.Skills
            {
                SkillSets = skills.SkillSets
            };

            string expModel = JsonConvert.SerializeObject(experienceModel);
            string skllsModel = JsonConvert.SerializeObject(skillsModel);
            return resumeBuilderRepository.InsertExperienceDetails(userId, expModel, skllsModel);
        }

        public bool InsertPersonalDetails(int userId, UserViewModel user)
        {
            var u = new Data.DataModel.Shared.UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address1 = user.Address1,
                Address2 = user.Address2,
                Address3 = user.Address3,
                City = user.City,
                CompanyName = user.CompanyName,
                Country = user.Country,
                DOB = user.DOB,
                Email = user.Email,
                Gender = user.Gender,
                MaritalStatus = user.MaritalStatus,
                MobileNo = user.MobileNo,
                State = user.State
            };
            return resumeBuilderRepository.InsertPersonalDetails(userId, u);
        }

        public bool InsertEducationDetails(int userId, EducationalDetails[] educations)
        {
            var eduModel = new Data.DataModel.JobSeeker.EducationalDetails[educations.Length];
            int i = 0;
            while (i < educations.Length)
            {
                eduModel[i] = new Data.DataModel.JobSeeker.EducationalDetails
                {
                    Id=educations[i].Id,
                    Course = educations[i].Course,
                    CourseType = educations[i].CourseType,
                    PassingYear = educations[i].PassingYear,
                    Percentage = educations[i].Percentage,
                    Qualification = educations[i].Qualification,
                    Specialization = educations[i].Specialization,
                    University = educations[i].University,
                };
                i++;
            }
            foreach (Data.DataModel.JobSeeker.EducationalDetails item in eduModel)
            {
                if(item!=null && item.Id <= 0)
                {
                    item.Id = eduModel.Where(e => e.Id > 0).ToList().Count + 1;
                }
            }
            string dataToSend = JsonConvert.SerializeObject(eduModel);
            return resumeBuilderRepository.InsertEducationDetails(userId, dataToSend);
        }

        public dynamic GetUserDetails(int userId)
        {
            dynamic returnData = new System.Dynamic.ExpandoObject();
            returnData.uDetail = new UserDetail();

            var dSet = resumeBuilderRepository.GetUserDetails(userId);
            if (null != dSet && dSet.Tables.Count > 0)
            {
                int tIndex = -1;
                foreach (DataTable table in dSet.Tables)
                {
                    tIndex++;
                    switch (tIndex)
                    {
                        case 0:
                            returnData.MaritalStatus = new List<MaritalStatusViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.MaritalStatus.Add(new MaritalStatusViewModel
                                {
                                    StatusId = Convert.ToInt32(row["StatusId"]),
                                    StatusCode = Convert.ToString(row["StatusCode"]),
                                    Status = Convert.ToString(row["Status"])
                                });
                            }
                            break;
                        case 1:
                            returnData.Gender = new List<GenderViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.Gender.Add(new GenderViewModel
                                {
                                    GenderId = Convert.ToInt32(row["GenderId"]),
                                    GenderCode = Convert.ToString(row["GenderCode"]),
                                    Gender = Convert.ToString(row["Gender"])
                                });
                            }
                            break;
                        case 2:
                            returnData.Countries = new List<CountryViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.Countries.Add(new CountryViewModel
                                {
                                    Country = Convert.ToString(row["Country"]),
                                    CountryCode = Convert.ToString(row["CountryCode"]),
                                });
                            }
                            break;
                        case 3:
                            returnData.uDetail.PersonalDetails = SerializeUserPersonalDetails(table.Rows[0]);
                            break;
                        case 4:
                            returnData.States = new List<StateViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.States.Add(new StateViewModel
                                {
                                    StateCode = Convert.ToString(row["StateCode"]),
                                    State = Convert.ToString(row["State"]),
                                });
                            }
                            break;
                        case 5:
                            returnData.Cities = new List<CityViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.Cities.Add(new CityViewModel
                                {
                                    CityCode = Convert.ToString(row["CityCode"]),
                                    City = Convert.ToString(row["City"]),
                                });
                            }
                            break;
                        case 6:
                            if (table.Rows.Count > 0)
                            {
                                returnData.uDetail.ExperienceDetails = JsonConvert.DeserializeObject<ExperienceDetails[]>(Convert.ToString(table.Rows[0]["ExperienceDetails"]));
                                returnData.uDetail.EducationalDetails = JsonConvert.DeserializeObject<EducationalDetails[]>(Convert.ToString(table.Rows[0]["EducationalDetails"]));
                                returnData.uDetail.Skills = JsonConvert.DeserializeObject<Skills>(Convert.ToString(table.Rows[0]["Skills"]));
                            }
                            break;
                        case 7:
                            returnData.AllCities = new List<CityViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.AllCities.Add(new CityViewModel
                                {
                                    CityCode = Convert.ToString(row["CityCode"]),
                                    City = Convert.ToString(row["City"]),
                                });
                            }
                            break;
                        case 8:
                            returnData.AllIndustries = new List<IndustryViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.AllIndustries.Add(new IndustryViewModel
                                {
                                    IndustryId = Convert.ToInt32(row["JobIndustryAreaId"]),
                                    Name = Convert.ToString(row["JobIndustryAreaName"]),
                                });
                            }
                            break;
                        case 9:
                            returnData.CourseCategories = new List<CourseCategoryViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.CourseCategories.Add(new CourseCategoryViewModel
                                {
                                    Id = Convert.ToInt32(row["CategoryId"]),
                                    Name = Convert.ToString(row["Name"]),
                                });
                            }
                            break;
                        case 10:
                            returnData.CourseTypes = new List<CourseTypeViewModel>();
                            foreach (DataRow row in table.Rows)
                            {
                                returnData.CourseTypes.Add(new CourseTypeViewModel
                                {
                                    TypeId = Convert.ToInt32(row["CourseTypeId"]),
                                    Type = Convert.ToString(row["Type"]),
                                });
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return returnData;
        }

        public IEnumerable<StateViewModel> GetStates(string cCode)
        {
            IList<StateViewModel> states = null;
            var dTable = masterDataRepository.GetStates(cCode);
            if (null != dTable && dTable.Rows.Count > 0)
            {
                states = new List<StateViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    states.Add(new StateViewModel
                    {
                        StateCode = Convert.ToString(row["StateCode"]),
                        State = Convert.ToString(row["State"])
                    });
                }
            }
            return states;
        }

        public IEnumerable<CityViewModel> GetCities(string sCode)
        {
            IList<CityViewModel> cities = null;
            var dTable = masterDataRepository.GetCities(sCode);
            if (null != dTable && dTable.Rows.Count > 0)
            {
                cities = new List<CityViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    cities.Add(new CityViewModel
                    {
                        CityCode = Convert.ToString(row["CityCode"]),
                        City = Convert.ToString(row["City"])
                    });
                }
            }
            return cities;
        }

        public IEnumerable<CourseViewModel> GetCourses(int cCategory)
        {
            IList<CourseViewModel> courses = null;
            var dTable = masterDataRepository.GetCourses(cCategory);
            if (null != dTable && dTable.Rows.Count > 0)
            {
                courses = new List<CourseViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    courses.Add(new CourseViewModel
                    {
                        Id = Convert.ToInt32(row["CourseId"]),
                        Name = Convert.ToString(row["Name"])
                    });
                }
            }
            return courses;
        }

        private UserViewModel SerializeUserPersonalDetails(DataRow userTbl)
        {
            double tExp = 0.0;
            if (!Convert.IsDBNull(userTbl["TotalExperience"]))
            {
                tExp = Convert.ToDouble(userTbl["TotalExperience"]);
            }
            return new UserViewModel
            {
                UserId = Convert.ToInt32(userTbl["UserId"]),
                FirstName = Convert.ToString(userTbl["FirstName"]),
                LastName = Convert.ToString(userTbl["LastName"]),
                Email = Convert.ToString(userTbl["Email"]),
                MobileNo = Convert.ToString(userTbl["MobileNo"]),
                Address1 = Convert.ToString(userTbl["Address1"]),
                Address2 = Convert.ToString(userTbl["Address2"]),
                Address3 = Convert.ToString(userTbl["Address3"]),
                Country = Convert.ToString(userTbl["Country"]),
                CountryName = Convert.ToString(userTbl["CountryName"]),
                State = Convert.ToString(userTbl["State"]),
                StateName = Convert.ToString(userTbl["StateName"]),
                City = Convert.ToString(userTbl["City"]),
                CityName = Convert.ToString(userTbl["CityName"]),
                MaritalStatus = Convert.ToString(userTbl["MaritalStatus"]),
                MaritalStatusName = Convert.ToString(userTbl["MaritalStatusName"]),
                Gender = Convert.ToString(userTbl["Gender"]),
                GenderName = Convert.ToString(userTbl["GenderName"]),
                DOB = Convert.ToString(userTbl["DateOfBirth"]),
                TotalExperience = tExp
            };
        }

        private void SerializeProfessionalDetails(IList<ExperienceDetails> expDetails)
        {
            if (null != expDetails && expDetails.Count > 0)
            {
                foreach (ExperienceDetails details in expDetails)
                {
                    if (!string.IsNullOrWhiteSpace(details.WorkLocation))
                        details.WorkLocationName = GetCityByCode(details.WorkLocation);
                    if (string.IsNullOrWhiteSpace(details.WorkingTill))
                    {
                        details.WorkingTill = "Present";
                    }
                }
            }
        }

        private void SerializeEducationalDetails(IList<EducationalDetails> expDetails)
        {
            if (null != expDetails && expDetails.Count > 0)
            {
                foreach (EducationalDetails details in expDetails)
                {
                    if (!string.IsNullOrWhiteSpace(details.Course))
                        details.CourseName = GetCourseById(details.Course);
                }
            }
        }

        private string GetCityByCode(string cityCode)
        {
            var row = masterDataRepository.GetCityByCode(cityCode);
            if (null != row)
            {
                return Convert.ToString(row["Name"]);
            }
            return string.Empty;
        }

        private string GetCourseById(string cId)
        {
            var dTable = masterDataRepository.GetCoursesById(Convert.ToInt32(cId));
            if (null != dTable && dTable.Rows.Count > 0)
            {
                return Convert.ToString(dTable.Rows[0]["CourseName"]);
            }
            return string.Empty;
        }
    }
}
