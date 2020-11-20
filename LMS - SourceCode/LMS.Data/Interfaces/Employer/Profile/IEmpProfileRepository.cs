using LMS.Data.DataModel.Shared;
using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Employer.Profile
{
    public interface IEmpProfileRepository
    {
        DataTable GetEmpUserDetails(int userId);
        bool InsertUpdateEmpDetails(UserModel model);
    }
}
