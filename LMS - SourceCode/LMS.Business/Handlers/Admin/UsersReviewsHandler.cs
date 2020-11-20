using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Admin;
using LMS.Data.DataModel.Admin.UserReviews;
using LMS.Data.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.UsersReviews;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Business.Handlers.Admin
{
   public class UsersReviewsHandler : IUsersReviewsHandler
    {
        private readonly IUsersReviewsRepository usersReviewsRepository;
        
        public UsersReviewsHandler(IConfiguration configuration)
        {
            var factory = new ProcessorFactoryResolver<IUsersReviewsRepository>(configuration);
            usersReviewsRepository = factory.CreateProcessor();
        }
        public List<UsersReviewsViewModel> GetUsersReviews()
        {
            DataTable dt = usersReviewsRepository.GetUsersReviews();
            List<UsersReviewsViewModel> totalReviewList = new List<UsersReviewsViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UsersReviewsViewModel reviewList = new UsersReviewsViewModel()
                {
                    Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                    Name = Convert.ToString(dt.Rows[i]["Name"]),
                    Email = Convert.ToString(dt.Rows[i]["Email"]),
                    Tagline = Convert.ToString(dt.Rows[i]["TagLine"]),
                    Message = Convert.ToString(dt.Rows[i]["Message"]),
                    CountValue = Convert.ToString(dt.Rows[i]["CountValue"]),
                   IsApprove = Convert.ToBoolean(dt.Rows[i]["IsApproved"])
                };
                totalReviewList.Add(reviewList);
            }
            return (totalReviewList);
        }
        public bool DeleteUsersReviews(string id, string deletedBy)
        {
            var result = usersReviewsRepository.DeleteUsersReviews(id, deletedBy);
            if (result)
            {
                return true;
            }
            throw new Exception("Unable to delete data");
        }
        public bool ApproveUsers(string id, string approvedBy)
        {
            var result = usersReviewsRepository.ApproveUsersReviews(id, approvedBy);
            if (result)
            {
                return true;
            }
            throw new Exception("Unable to delete data");
        }
        public bool UpdateUserReview(UsersReviewsViewModel data,string userid)
        {
            var model = new UserReviewsModel()
            {
                Id = data.Id,
                Name = data.Name,
                Tagline = data.Tagline,
                Message = data.Message,
                Email  = data.Email
            };
            var result = usersReviewsRepository.UpdateUsersReviews(model,userid);
            if (result)
            {
                return true;
            }
            throw new Exception("Unable to Update data");
        }
    }
}
