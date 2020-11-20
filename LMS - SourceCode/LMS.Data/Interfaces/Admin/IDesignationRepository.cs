using LMS.Data.DataModel.Admin.Designation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface IDesignationRepository
    {
        DataTable GetDesignationList();
        bool AddDesignation(DesignationModel designationModel);
        bool UpdateDesignation(DesignationModel designationModel);
        bool DeleteDesignation(int designatioId);
    }
}
