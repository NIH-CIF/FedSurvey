using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FedSurvey.Models
{
    [NotMapped]
    public partial class ResponseDTO
    {
        // Consider exposing created at and updated at too if it could help debugging.
        public int Id { get; set; }
        public int QuestionExecutionId { get; set; }
        public int PossibleResponseId { get; set; }
        public int DataGroupId { get; set; }
        public decimal Count { get; set; }
        public decimal Percentage { get; set; }
    }
}
