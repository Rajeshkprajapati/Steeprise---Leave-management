using LMS.Data.DataModel.Admin.UserReviews;
using LMS.Model.DataViewModel.Admin.UsersReviews;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface IUsersReviewsRepository
    {
        DataTable GetUsersReviews();
        bool UpdateUsersReviews(UserReviewsModel usersReviews,string userid);
        bool DeleteUsersReviews(string id, string deletedBy);
        bool ApproveUsersReviews(string id, string approvedBy);
    }
}
