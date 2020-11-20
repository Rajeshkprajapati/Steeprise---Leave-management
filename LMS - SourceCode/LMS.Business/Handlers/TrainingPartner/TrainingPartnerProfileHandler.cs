using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.TrainingPartner;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.TrainingPartner;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LMS.Business.Handlers.TrainingPartner
{
    public class TrainingPartnerProfileHandler : ITrainingPartnerProfileHandler
    {
        private readonly ITrainingPartnerProfileRepository _trainingPartnerProfileRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public TrainingPartnerProfileHandler(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            var factory = new ProcessorFactoryResolver<ITrainingPartnerProfileRepository>(configuration);
            _trainingPartnerProfileRepository = factory.CreateProcessor();
            _hostingEnvironment = hostingEnvironment;
        }
        public UserViewModel GetTPDetail(int userid)
        {
            var tpdetails = _trainingPartnerProfileRepository.GetTPDetail(userid);
            UserViewModel user = new UserViewModel();
            if (tpdetails != null && tpdetails.Rows.Count > 0)
            {
                foreach (DataRow row in tpdetails.Rows)
                {
                    user.UserId = row["UserId"] as int? ?? 0;
                    user.CandidateId = row["Candidateid"] as string ?? "";
                    user.FirstName = row["FirstName"] as string ?? "";
                    user.LastName = row["LastName"] as string ?? "";
                    user.Email = row["Email"] as string ?? "";
                    user.ProfilePic = row["ProfilePic"] as string ?? "";
                }
            }
            return user;
        }
        public bool UpdateTPDetail(UserViewModel user)
        {
            var filename = user.ImageFile.FileName;
            string fName = $@"\ProfilePic\{user.Email + "_" + filename}";
            filename = _hostingEnvironment.WebRootPath + fName;
            using (FileStream fs = File.Create(filename))
            {
                user.ImageFile.CopyTo(fs);
            };
            user.ProfilePic = fName;
            UserModel model = new UserModel()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePic = user.ProfilePic
            };
            return _trainingPartnerProfileRepository.UpdateTPDetail(model);
        }
    }
}
