using LMS.Model.DataViewModel.Admin.UsersReviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface IUsersReviewsHandler
    {
        List<UsersReviewsViewModel> GetUsersReviews();
        bool DeleteUsersReviews(string id, string deletedBy);
        bool UpdateUserReview(UsersReviewsViewModel model,string userid);
        bool ApproveUsers(string id, string approvedBy);
    }
}
