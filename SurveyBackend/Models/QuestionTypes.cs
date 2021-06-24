using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class QuestionTypes
    {
        public QuestionTypes()
        {
            PossibleResponses = new HashSet<PossibleResponses>();
            Questions = new HashSet<Questions>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("QuestionType")]
        public virtual ICollection<PossibleResponses> PossibleResponses { get; set; }
        [InverseProperty("QuestionType")]
        public virtual ICollection<Questions> Questions { get; set; }
    }
}
