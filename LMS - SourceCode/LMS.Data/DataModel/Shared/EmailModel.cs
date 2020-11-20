using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Shared
{
    public class EmailModel
    {
        public int MailId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int InsertedBy { get; set; }
        public int MailType { get; set; }
    }
}
