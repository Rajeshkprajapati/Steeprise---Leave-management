using LMS.Data.DataModel.Admin.ManageUsers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface IManageUserRepository
    {
        DataTable GetAllUsersList(int userId);
        DataTable GetAllUserRegistrations(string registrationType, string sDate, string eDate);
        DataTable GetAppliedJobsInRange(string startDate, string endDate);
        DataTable GetJobsInDateRange(string startDay, string endDay);
        DataTable MonthlyAppliedJobs(int month, int year, string gender, string state);
        DataTable MonthlyRegisteredUsers(int month, int year,string state, string gender);
        bool DeleteUsersById(string userid);
        DataSet DashboardTilesRecordCount(string date,string endDate);
        bool UpdateUsersData(ManageUsersModel user);
        bool ApproveUser(int id);
        DataSet GetGraphData(int year, string gender, string state);
        DataTable MonthlyJobs(int month, int year, string state, bool activeJobs);
        DataTable JobPostMonthlyStateWiseRecord(string month, string year, string state);
        DataTable GetBulkJobSearchList(int CompanyId, string FY, string statecode, string citycode);
        bool DeleteBulkJobPost(string JobPostId);
    }
}
