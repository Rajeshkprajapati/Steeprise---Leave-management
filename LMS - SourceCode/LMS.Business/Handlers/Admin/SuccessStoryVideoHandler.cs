using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Admin;
using LMS.Data.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.SuccessStory;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LMS.Business.Handlers.Admin
{
    public class SuccessStoryVideoHandler : ISuccessStoryVideoHandler
    {
        private readonly ISuccessStoryVideoRepository successStoryVideoRepository;
        private IHostingEnvironment hostingEnviroment;
        public SuccessStoryVideoHandler(IConfiguration configuration, IHostingEnvironment _hostingEnvironment)
        {
            var factory = new ProcessorFactoryResolver<ISuccessStoryVideoRepository>(configuration);
            successStoryVideoRepository = factory.CreateProcessor();
            hostingEnviroment = _hostingEnvironment;
        }
        public List<SuccessStoryVideoViewModel> GetSuccessStoryVid()
        {
            DataTable dt = successStoryVideoRepository.GetSuccessStoryVid();
            List<SuccessStoryVideoViewModel> ssVideoList = new List<SuccessStoryVideoViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SuccessStoryVideoViewModel ssVideo = new SuccessStoryVideoViewModel()
                {
                    Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                    Title = Convert.ToString(dt.Rows[i]["Title"]),
                    Type = Convert.ToString(dt.Rows[i]["Type"]),
                    VideoFile = Convert.ToString(dt.Rows[i]["FileName"]),
                    //SerialNo = Convert.ToString(dt.Rows[i]["SerialNo"]),
                    DisplayOrder = Convert.ToInt16(dt.Rows[i]["DisplayOrder"]),
                };
                ssVideoList.Add(ssVideo);
            }
            return (ssVideoList);
        }
        public bool InsertUpdateSuccessStoryVid(SuccessStoryVideoViewModel successStory, string updatedBy)
        {

            SuccessStoryVideoViewModel ssVid = new SuccessStoryVideoViewModel
            {
                Id = successStory.Id,
                Title = successStory.Title,
                Type = successStory.Type,
                VideoFile = successStory.VideoFile,
                DisplayOrder = successStory.DisplayOrder,
                UpdatedBy = Convert.ToString(updatedBy)
            };
            var result = successStoryVideoRepository.InsertUpdateSuccessStoryVid(ssVid);
            if (result)
            {
                return true;
            }
            throw new DataNotUpdatedException("Unable to update data");
        }
        public bool DeleteSuccessStoryVid(string SSId, string deletedBy)
        {
            var result = successStoryVideoRepository.DeleteSuccessStoryVid(SSId, deletedBy);
            if (result)
            {
                return true;
            }
            throw new DataNotUpdatedException("Unable to delete data");
        }
    }
}
