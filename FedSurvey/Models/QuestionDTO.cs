using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FedSurvey.Models
{
    [NotMapped]
    public partial class QuestionDTO
    {
        public int Id { get; set; }
        public string Body { get; set; }
    }
}
