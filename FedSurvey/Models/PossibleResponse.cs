using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public bool PartOfPercentage { get; set; } = true;

        [ForeignKey(nameof(QuestionTypeId))]
        [InverseProperty(nameof(Models.QuestionType.PossibleResponses))]
        public virtual QuestionType QuestionType { get; set; }
        [InverseProperty("PossibleResponse")]
        public virtual ICollection<PossibleResponseString> PossibleResponseStrings { get; set; }
        [InverseProperty("PossibleResponse")]
        public virtual ICollection<Response> Responses { get; set; }

        // I believe the API should pretty entirely abstract away PossibleResponseStrings, at least to start.
        public class DTO
        {
            public int Id { get; set; }
            public int QuestionTypeId { get; set; }
            public string Name { get; set; }
            public bool PartOfPercentage { get; set; }
        }

        public static DTO ToDTO(PossibleResponse possibleResponse)
        {
            string name = possibleResponse.PossibleResponseStrings.Count > 0 ? possibleResponse.PossibleResponseStrings.FirstOrDefault().Name : null;

            return new DTO
            {
                Id = possibleResponse.Id,
                QuestionTypeId = possibleResponse.QuestionTypeId,
                Name = name,
                PartOfPercentage = possibleResponse.PartOfPercentage
            };
        }
    }
}
