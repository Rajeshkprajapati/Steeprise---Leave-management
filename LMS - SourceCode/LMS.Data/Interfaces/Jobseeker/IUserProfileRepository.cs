using LMS.Data.DataModel.JobSeeker;
using LMS.Data.DataModel.Shared;
using LMS.Model.DataViewModel.JobSeeker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Jobseeker
{
    public interface IUserProfileRepository
    {
        bool AddNewExperience(int userId,string model);
        bool AddNeweducational(int userId, string model);
        bool AddNewSkills(int userId, string skills);
        DataTable GetAllCities();
        bool AddNewProfileSummary(string profile, int userId);
        DataTable GetJobseekerData(int UserId);
        bool AddNewProfileDetail(UserModel model);
        bool AddNewUploadFile(string fName, int userId);
        bool UploadProfilePicture(string fName, int userId);
        bool ApplyJob(int userId, int jobPostId);
        bool CheckIfJobExist(string UserId, string jobPostId);
        bool CheckIfskillEmpty(string UserId);
        bool CheckIfResumeEmpty(string UserId);
        bool CheckIfEducationDetailsEmpty(string UserId);
        bool CheckIfUserAvailableInUserProfessionalDetails(string UserId);
        DataTable GetEmployerDetailFromJobId(int jobId);
        DataTable GetUserPreferredlocation(int userid);
        DataTable GetUserITSkills(int userid);
        bool UpdateItSkills(ITSkills ITSkills, int UserId);
        bool DeleteITSkill(int ITSkillId, int UserId);
        DataSet GetJobseekerDashboard(int userId);
        DataTable GetJobseekerAppliedJobs(int userId);
        DataTable GetJobseekerViewedProfile(int userId);
        bool DeleteAppliedJob(int JobPostId, int UserId);
        DataTable EmployerFollowers(int userId);
        bool UnfollowEmployer(int EmployerId, int UserId);
        DataTable JobSeekerSkills(int userId);
        DataTable JobSeekerJobsOnSkills(string skills,int UserId);
        bool JobsAlert(int JobAlert, int UserId);
        DataTable ProfileScore(int UserId);
        DataTable JobSeekerContacted(int userId);
    }
}
