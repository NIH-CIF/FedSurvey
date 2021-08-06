using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FedSurvey.Models
{
    [NotMapped]
    public partial class MergeCandidateDTO
    {
        public int QuestionId { get; set; }
        public string ExecutionKey { get; set; }
        public string Body { get; set; }
        public int Position { get; set; }
    }
}
