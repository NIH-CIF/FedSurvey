using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class QuestionType : Timestamps
    {
        public QuestionType()
        {
            PossibleResponses = new HashSet<PossibleResponse>();
            Questions = new HashSet<Question>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("QuestionType")]
        public virtual ICollection<PossibleResponse> PossibleResponses { get; set; }
        [InverseProperty("QuestionType")]
        public virtual ICollection<Question> Questions { get; set; }

        [InverseProperty("QuestionType")]
        public virtual ICollection<QuestionTypeString> QuestionTypeStrings { get; set; }
    }
}
