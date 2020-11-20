using LMS.Model.DataViewModel.Admin.SuccessStory;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
  public interface ISuccessStoryVideoHandler
    {
        List<SuccessStoryVideoViewModel> GetSuccessStoryVid();
        bool InsertUpdateSuccessStoryVid(SuccessStoryVideoViewModel successStory,string updatedBy);
        bool DeleteSuccessStoryVid(string SSId, string deletedBy);
    }
}
