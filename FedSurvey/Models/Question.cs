﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class Question : Timestamps
    {
        public Question()
        {
            QuestionExecutions = new HashSet<QuestionExecution>();
            QuestionLinks = new HashSet<QuestionLink>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }

        [ForeignKey(nameof(QuestionTypeId))]
        [InverseProperty(nameof(Models.QuestionType.Questions))]
        public virtual QuestionType QuestionType { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionExecution> QuestionExecutions { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionLink> QuestionLinks { get; set; }
    }
}
