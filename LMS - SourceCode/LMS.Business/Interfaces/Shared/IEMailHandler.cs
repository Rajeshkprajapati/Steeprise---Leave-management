using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Shared
{
    public interface IEMailHandler
    {
        void SendMail(EmailViewModel email, int userId,bool isInsertInDB=true);
        bool IsValidEmail(string email);
    }
}
