using LMS.Model.DataViewModel.Employer.Dashboard;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Employer
{
    public interface IDashboardHandler
    {
        DashboardSummary GetDashboard(int empId);
        UserViewModel GetProfileData(int empId);
        IEnumerable<JobPostViewModel> GetJobs(int empId, int year);
        IEnumerable<Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel> GetJobSeekers(int empId);
        IEnumerable<UserViewModel> GetViewedProfiles(int empId);
        IEnumerable<UserViewModel> GetJobSeekersBasedOnEmployerHiringCriteria(int empId, string year, string city, string role);
        JobPostViewModel GetJob(int jobId,int empId);
        IEnumerable<StateViewModel> GetStates(string cCode);
        IEnumerable<CityViewModel> GetCities(string sCode);
        IEnumerable<CountryViewModel> GetCountries();
        IEnumerable<JobTitleViewModel> GetJobRoles();
        bool UpdateJob(JobPostViewModel job,int userId);
        IEnumerable<UserViewModel> GetEmployers();
        IEnumerable<MessageViewModel> GetMessages(DateTime msgsOnDate, int empId);
        bool ReplyToJobSeeker(MessageViewModel msg, int userId);
        List<CityViewModel> GetCityListWithoutState();
    }
}
