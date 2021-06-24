using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class Questions
    {
        public Questions()
        {
            QuestionExecutions = new HashSet<QuestionExecutions>();
        }

        [Key]
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }

        [ForeignKey(nameof(QuestionTypeId))]
        [InverseProperty(nameof(QuestionTypes.Questions))]
        public virtual QuestionTypes QuestionType { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionExecutions> QuestionExecutions { get; set; }
    }
}
