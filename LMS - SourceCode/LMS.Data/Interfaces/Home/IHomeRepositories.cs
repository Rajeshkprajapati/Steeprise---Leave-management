using LMS.Model.DataViewModel.Home;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Home
{
    public interface IHomeRepositories
    {
        DataTable GetCityHasJobPostId();
        DataTable GetCitiesWithJobSeekerInfo();
        DataTable GetCityListDetail();
        DataTable GetCityListByChar(string cityFirstChar);
        DataTable GetJobListByChar(string jobFirstChar);
        DataTable GetSuccessStory();
        bool PostSuccsessStory(SuccessStoryViewModel successStory);
        DataTable GetFeaturedJobs();
        DataTable ViewAllFeaturedJobs();
        DataTable PopulerSearchesCategory();
        DataTable PopulerSearchesCity();
        DataTable AllJobsByCategory(int categoryId);
        DataTable AllJobsByCity(string CityCode);
        DataTable GetCategory();
        DataTable TopEmployer();
        DataTable GetAllCompanyList();
        DataTable NasscomJobsList();
        DataTable GetAplliedJobs(int userid);
        DataTable GetSuccessStoryVideo();
        DataTable GetCompanyHasJobPostId();
        string GetContactUsEmail();
        string TalentConnectLink();
        string CandidateBulkUpload();
        string TPRegistrationGuide();
        DataTable GetRecentJobs();
        DataTable GetWalkInJobs();
        bool EmployerFollower(int EmployeeId, int UserId);
        DataTable CategoryJobVacancies();
        DataTable CityJobVacancies();
        DataTable CompanyJobVacancies();
        DataTable AllJobsByCompany(int UserId);
    }
}
