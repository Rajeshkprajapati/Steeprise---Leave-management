using LMS.Data.DataModel.Employer.JobPost;
using LMS.Model.DataViewModel.Employer.JobPost;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Employer.JobPost
{
    public interface IJobPostRepository
    {
        DataTable GetJobIndustryAreaDetail();
        DataTable GetJobIndustryAreaWithJobPost();
        DataTable GetJobIndustryAreaWithStudentData();
        DataTable GetJobJobEmploymentStatusDetail();
        DataTable GetJobJobEmploTypeDetail();
        DataTable GetStateListDetail(string CountryCode);
        DataTable GetCityListDetail(string StateCode);
        DataTable GetGenderListDetail();
        bool AddJobPostData(JobPostModel model);
        DataTable GetJobDetails(int jobid);
        bool AddPreferredLocation(string location,int i,int userid);
        DataTable RecommendedJobs(int roleid);
    }
}
