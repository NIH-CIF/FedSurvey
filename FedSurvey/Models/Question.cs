using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FedSurvey.Models
{
    public partial class Question
    {
        public Question()
        {
            QuestionExecutions = new HashSet<QuestionExecution>();
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
    }
}
