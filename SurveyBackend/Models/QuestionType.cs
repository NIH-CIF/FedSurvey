using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            PossibleResponses = new HashSet<PossibleResponse>();
            Questions = new HashSet<Question>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("QuestionType")]
        public virtual ICollection<PossibleResponse> PossibleResponses { get; set; }
        [InverseProperty("QuestionType")]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
