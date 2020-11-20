using System.Net.Mail;

namespace LMS.Model.DataViewModel.Shared
{
    public class EmailViewModel
    {
        public string From { get; set; }
        public string[] To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public int MailType { get; set; }
    }
}
