using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.TrainingPartner
{
    public class CandidatesViewModel
    {
        public int Id { get; set; }
        public string CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
