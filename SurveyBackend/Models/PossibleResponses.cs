using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class PossibleResponses
    {
        public PossibleResponses()
        {
            PossibleResponseStrings = new HashSet<PossibleResponseStrings>();
            Responses = new HashSet<Responses>();
        }

        [Key]
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }

        [ForeignKey(nameof(QuestionTypeId))]
        [InverseProperty(nameof(QuestionTypes.PossibleResponses))]
        public virtual QuestionTypes QuestionType { get; set; }
        [InverseProperty("PossibleResponse")]
        public virtual ICollection<PossibleResponseStrings> PossibleResponseStrings { get; set; }
        [InverseProperty("PossibleResponse")]
        public virtual ICollection<Responses> Responses { get; set; }
    }
}
