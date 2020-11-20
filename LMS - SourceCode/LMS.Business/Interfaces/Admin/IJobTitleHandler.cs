using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
   public interface IJobTitleHandler
    {
        List<JobTitleViewModel> GetJobTitle();
        bool InsertUpdateJobTile(JobTitleViewModel jobTitle);
        bool DeleteJobTitle(string jobTileId, string deletedBy);
    }
}
