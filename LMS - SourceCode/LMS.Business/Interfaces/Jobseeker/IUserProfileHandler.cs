using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Jobseeker
{
    public interface IUserProfileHandler
    {
        bool AddExperienceDetails(int userId, ExperienceDetails[] model);
        bool AddEducationalDetailsDetails(int userId, EducationalDetails[] model);
        bool AddSkillDetails(int userId, Skills model);
        bool AddProfileSummaryDetails(string profile, int userId);
        UserDetail GetJobseekerDetail(int UserId);
        bool AddProfileDetails(UserViewModel model);
        bool UploadFileData(string fName, int userId);
        bool UploadProfilePicture(string fName, int userId);
        bool ApplyJobDetails(UserViewModel user, int jobPostId);
        IList<JobTypeViewModel> GetJobTypes();
        bool UpdateItSkills(ITSkills ITSkills, int UserId);
        bool DeleteITSkill(int ITSkillId,int UserId);
        JobSeekerDashboardSummary GetJobSeekerDashboard(int UserId);
        List<SearchJobListViewModel> GetJobseekerAppliedJobs(int userId);
        List<JobSeekerViewedProfile> GetJobseekerViewedProfile(int userId);
        bool DeleteAppliedJob(int JobPostId, int UserId);
        List<EmployerFollowers> EmployerFollowers(int userId);
        bool UnfollowEmployer(int EmployerId, int UserId);
        Skills JobSeekerSkills(int userId);
        List<SearchJobListViewModel> JobSeekerJobsOnSkills(string skills,int UserId);
        bool JobsAlerts(int IsAlert, int UserId);
        List<MessageViewModel> JobSeekerContacted(int userId);
    }
}

