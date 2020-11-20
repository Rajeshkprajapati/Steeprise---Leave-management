using LMS.Data.DataModel.JobSeeker;
using LMS.Data.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Jobseeker
{
    public interface IResumeBuilderRepository
    {
        DataSet GetUserDetails(int userId);
        DataSet GetUserDetailsForResumeBuilder(int userId);
        bool InsertExperienceDetails(int userId,string exp,string skills);
        bool InsertEducationDetails(int userId, string educations);
        bool InsertPersonalDetails(int userId, UserModel user);
        bool UpdateResumePath(int userId, string resumePath);
        string GetResume(int userId);
    }
}
