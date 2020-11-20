using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Employer.SearchResume;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Employer.SearchResume;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Employer.SearchResume;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LMS.Business.Handlers.Employer.SearchResume
{
    public class SearchResumeHandler : ISearchResumeHandler
    {
        private readonly ISearchResumeRepository _serarchresumeProcess;
        private IHostingEnvironment _hostingEnviroment;
        private readonly IMasterDataRepository _masterRepository;
        public SearchResumeHandler(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            var factory = new ProcessorFactoryResolver<ISearchResumeRepository>(configuration);
            _serarchresumeProcess = factory.CreateProcessor();
            var masterfactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            _masterRepository = masterfactory.CreateProcessor();
            _hostingEnviroment = hostingEnvironment;
        }
        public List<SearchResumeListViewModel> GetSearchResumeList(SearchResumeViewModel searches)
        {
            List<SearchResumeListViewModel> lstSearchedResume = new List<SearchResumeListViewModel>();
            var sFilters = new SearchResumeModel
            {
                Skills = searches.Skills,
                JobCategory = string.Join(Constants.CommaSeparator, searches.JobCategory),
                City = string.Join(Constants.CommaSeparator, searches.City),
                MinExp = searches.MinExp,
                MaxExp = searches.MaxExp
            };
            DataTable searchedResume = _serarchresumeProcess.GetSearchResumeList(sFilters);
            if (searchedResume.Rows.Count > 0)
            {
                for (int i = 0; i < searchedResume.Rows.Count; i++)
                {
                    string resumePath = Convert.ToString(searchedResume.Rows[i]["Resume"]);
                    if (!string.IsNullOrWhiteSpace(resumePath))
                    {
                        if (!File.Exists($"{_hostingEnviroment.WebRootPath}{resumePath}"))
                        {
                            resumePath = string.Empty;
                        }
                    }

                    var skillsObject = new SearchResumeListViewModel
                    {
                        Skills = JsonConvert.DeserializeObject<Skills>(Convert.ToString(searchedResume.Rows[i]["Skills"])),                        
                        FirstName = (searchedResume.Rows[i]["FirstName"] as string) ?? "",
                        LastName = (searchedResume.Rows[i]["LastName"] as string) ?? "",
                        Email = (searchedResume.Rows[i]["Email"] as string) ?? "",
                        Resume = resumePath,
                        UserId = (searchedResume.Rows[i]["UserId"] as int?) ?? 0,
                        CityName = (searchedResume.Rows[i]["CityName"] as string) ?? "",
                        JobIndustryAreaName = (searchedResume.Rows[i]["JobIndustryAreaName"] as string) ?? "",
                        JobTitle = (searchedResume.Rows[i]["JobTitleName"] as string) ?? "",
                        //Address = (searchedResume.Rows[i]["Address"] as string) ?? "",
                        AboutMe = (searchedResume.Rows[i]["AboutMe"] as string) ?? "",
                        ProfilePic = Convert.ToString(searchedResume.Rows[i]["ProfilePic"]),
                    };
                    //var len = skillsObject.Skills.SkillSets.Length;
                    //if (skillsObject.Skills != null && skillsObject.Skills.SkillSets.Substring(len-1) != ",")
                    //{
                    //    skillsObject.Skills.SkillSets += ",";
                    //}
                    //skillsObject.Skills.SkillSets += Convert.ToString(searchedResume.Rows[i]["ITSkill"]);
                    lstSearchedResume.Add(skillsObject);
                    string picpath = Path.GetFullPath(_hostingEnviroment.WebRootPath + lstSearchedResume[i].ProfilePic);
                    if (!System.IO.File.Exists(picpath))
                    {
                        string fName = $@"\ProfilePic\" + "Avatar.jpg";
                        lstSearchedResume[i].ProfilePic = fName;
                    }
                }
                return lstSearchedResume;
            }
            throw new UserNotFoundException("Data Not found");
        }


        public SearchResumeListViewModel ShowCandidateDetails(int employerId, int jobSeekerId)
        {
            //var a = JsonConvert.DeserializeObject(skill).ToString();
            SearchResumeListViewModel model = new SearchResumeListViewModel();
            DataTable searchedResume = _serarchresumeProcess.ShowCandidateDetails(employerId, jobSeekerId);
            if (searchedResume.Rows.Count > 0)
            {
                EducationalDetails[] objEducationDetail = JsonConvert.DeserializeObject<EducationalDetails[]>(searchedResume.Rows[0]["EducationalDetails"].ToString());
                ExperienceDetails[] objExperience = JsonConvert.DeserializeObject<ExperienceDetails[]>(searchedResume.Rows[0]["ExperienceDetails"].ToString());
                model.Skills = new Skills();
                var skills = JsonConvert.DeserializeObject<Skills>(searchedResume.Rows[0]["Skills"].ToString());
                if (null != skills)
                {
                    model.Skills = skills;
                }
                string resumePath = Convert.ToString(searchedResume.Rows[0]["Resume"]);
                if (!string.IsNullOrWhiteSpace(resumePath))
                {
                    if (!File.Exists($"{_hostingEnviroment.WebRootPath}{resumePath}"))
                    {
                        resumePath = string.Empty;
                    }
                }
                model.ExperienceDetails = objExperience;
                model.EducationalDetails = objEducationDetail;
                if (model.EducationalDetails != null)
                {
                    foreach (EducationalDetails edu in model.EducationalDetails)
                    {
                        DataTable coursename = _masterRepository.GetCoursesById(Convert.ToInt32(edu.Course));
                        if (coursename != null && coursename.Rows.Count>0)
                        {
                            edu.CourseName = coursename.Rows[0]["CourseName"] as string ?? "";
                        }
                    }
                }

                model.FirstName = Convert.ToString(searchedResume.Rows[0]["FirstName"]);
                model.LastName = Convert.ToString(searchedResume.Rows[0]["LastName"]);
                model.Email = Convert.ToString(searchedResume.Rows[0]["Email"]);
                model.Resume = resumePath;
                model.UserId = Convert.ToInt32(searchedResume.Rows[0]["UserId"]);
                model.CityCode = Convert.ToString(searchedResume.Rows[0]["CityCode"]);
                model.CityName = Convert.ToString(searchedResume.Rows[0]["CityName"]);
                model.JobIndustryAreaName = Convert.ToString(searchedResume.Rows[0]["JobIndustryAreaName"]);
                model.CreatedOn = Convert.ToDateTime(searchedResume.Rows[0]["CreatedOn"]);
                model.Address = Convert.ToString(searchedResume.Rows[0]["Address1"]);
                model.State = Convert.ToString(searchedResume.Rows[0]["State"]);
                model.StateName = Convert.ToString(searchedResume.Rows[0]["StateName"]);
                model.Country = Convert.ToString(searchedResume.Rows[0]["Country"]);
                model.MobileNo = Convert.ToString(searchedResume.Rows[0]["MobileNo"]);
                model.ProfilePic = Convert.ToString(searchedResume.Rows[0]["ProfilePic"]);
                model.AboutMe = searchedResume.Rows[0]["AboutMe"].ToString();
                model.LinkedinProfile = searchedResume.Rows[0]["LinkedinProfile"].ToString();
                model.JobTitle = searchedResume.Rows[0]["JobTitleName"].ToString();
                model.CountryName = searchedResume.Rows[0]["CountryName"].ToString();
                //model.TotalExperience = Convert.ToDouble(searchedResume.Rows[0]["TotalExperience"]);
                if (!Convert.IsDBNull(searchedResume.Rows[0]["TotalExperience"]))
                {
                    model.TotalExperience = Convert.ToDouble(searchedResume.Rows[0]["TotalExperience"]);
                }
                string dob = Convert.ToString(searchedResume.Rows[0]["DateOfBirth"] as string) ?? "";

                if (dob != null && dob != "")
                {
                    DateTime date = Convert.ToDateTime(dob);
                    model.DateOfBirth = Convert.ToString(DateTime.Now.Year - date.Year);
                }
                model.CurrentSalary = searchedResume.Rows[0]["CurrentSalary"].ToString();
                model.ExpectedSalary = searchedResume.Rows[0]["ExpectedSalary"].ToString();

                return model;
            }
            throw new UserNotFoundException("Data Not found");
        }
    }
}
