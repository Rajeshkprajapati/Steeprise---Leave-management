using LMS.Model.DataViewModel.Admin.Designation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface IDesignationHandler
    {
        List<DesignationViewModel> GetDesignationList();

        bool AddDesignation(DesignationViewModel designationModel);

        bool UpdateDesignation(DesignationViewModel designationModel);

        bool DeleteDesignation(int designationId);

    }
}
