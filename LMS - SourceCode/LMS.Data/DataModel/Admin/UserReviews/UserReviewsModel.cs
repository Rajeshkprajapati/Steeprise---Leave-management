using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Admin.UserReviews
{
    public class UserReviewsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tagline { get; set; }
        public string Message { get; set; }
        public string City { get; set; }
        public string CountValue { get; set; }
        public string ProfilePic { get; set; }
    }
}
