using LMS.Data.DataModel.Employer.JobPost;
using LMS.Data.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Employer
{
    public interface IDashboardRepository
    {
        DataTable GetProfileData(int empId);
        DataTable GetJobs(int empId, int year, int jobId = 0);
        DataTable GetJobSeekers(int empId, int jobId=0);
        DataSet GetDashboard(int empId);
        DataTable GetViewedProfiles(int empId);
        DataTable GetJobSeekersBasedOnEmployerHiringCriteria(int empId, string year, string city, string role);
        DataTable GetJob(int jobId);
        bool UpdateJob(int userId, int jobId, JobPostModel job);
        DataTable GetMessages(DateTime msgsOnDate, int empId);
        bool UpdateJobSeekerMailStatus(int messageId,int userId);
        //DataTable GetJobSeekersByCity(string cityCode);
        //DataTable GetJobSeekersByYear(string year);
    }
}
