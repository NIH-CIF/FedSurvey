using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FedSurvey.Models
{
    public partial class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Count { get; set; }
        public int QuestionExecutionId { get; set; }
        public int PossibleResponseId { get; set; }
        public int DataGroupId { get; set; }

        [NotMapped]
        public decimal Total { get; set; }

        [ForeignKey(nameof(DataGroupId))]
        [InverseProperty(nameof(Models.DataGroup.Responses))]
        public virtual DataGroup DataGroup { get; set; }
        [ForeignKey(nameof(PossibleResponseId))]
        [InverseProperty(nameof(Models.PossibleResponse.Responses))]
        public virtual PossibleResponse PossibleResponse { get; set; }
        [ForeignKey(nameof(QuestionExecutionId))]
        [InverseProperty(nameof(Models.QuestionExecution.Responses))]
        public virtual QuestionExecution QuestionExecution { get; set; }

        public class DTO
        {
            public int Id { get; set; }
            public int QuestionExecutionId { get; set; }
            public int PossibleResponseId { get; set; }
            public int DataGroupId { get; set; }
            public decimal Percentage { get; set; }
        }

        public static DTO ToDTO(Response response)
        {
            return new DTO
            {
                Id = response.Id,
                QuestionExecutionId = response.QuestionExecutionId,
                PossibleResponseId = response.PossibleResponseId,
                DataGroupId = response.DataGroupId,
                Percentage = response.Count / response.Total * 100
            };
        }
    }
}
