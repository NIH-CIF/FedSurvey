using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FedSurvey.Models
{
    public partial class PossibleResponse
    {
        public PossibleResponse()
        {
            PossibleResponseStrings = new HashSet<PossibleResponseString>();
            Responses = new HashSet<Response>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }

        [ForeignKey(nameof(QuestionTypeId))]
        [InverseProperty(nameof(Models.QuestionType.PossibleResponses))]
        public virtual QuestionType QuestionType { get; set; }
        [InverseProperty("PossibleResponse")]
        public virtual ICollection<PossibleResponseString> PossibleResponseStrings { get; set; }
        [InverseProperty("PossibleResponse")]
        public virtual ICollection<Response> Responses { get; set; }

        public class DTO
        {
            public int Id { get; set; }
            public int QuestionTypeId { get; set; }
        }

        public static DTO ToDTO(PossibleResponse possibleResponse)
        {
            return new DTO
            {
                Id = possibleResponse.Id,
                QuestionTypeId = possibleResponse.QuestionTypeId
            };
        }
    }
}
