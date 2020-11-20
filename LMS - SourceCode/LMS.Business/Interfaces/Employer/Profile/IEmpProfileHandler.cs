using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Employer.Profile
{
    public interface IEmpProfileHandler
    {
        UserViewModel GetEmpProfileDetail(int userId);
        bool InsertUpdateEmpDetail(UserViewModel model);
        List<GenderViewModel> GetAllGenders();
        List<MaritalStatusViewModel> GetMaritalStatusMaster();
    }
}
