using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string SenderFName { get; set; }
        public string SenderLName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SenderFullName { get { return $"{SenderFName} {SenderLName}"; } }
        public string SenderMobile { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public bool IsReplied { get; set; }
    }
}
