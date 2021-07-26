using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FedSurvey.Models
{
    [NotMapped]
    public partial class ResultDTO
    {
        public string ExecutionName { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string PossibleResponseName { get; set; }
        public decimal Count { get; set; }
        public decimal? Percentage { get; set; }
        public string QuestionText { get; set; }
        public string DataGroupName { get; set; }
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
    }
}
