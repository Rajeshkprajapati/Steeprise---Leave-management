using LMS.Data.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.Interfaces.Shared
{
    public interface IEmailRepository
    {
        bool SaveMailInformation(EmailModel email);
    }
}
