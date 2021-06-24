using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class Responses
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Count { get; set; }
        public int QuestionExecutionId { get; set; }
        public int PossibleResponseId { get; set; }
        public int DataGroupId { get; set; }

        [ForeignKey(nameof(DataGroupId))]
        [InverseProperty(nameof(DataGroups.Responses))]
        public virtual DataGroups DataGroup { get; set; }
        [ForeignKey(nameof(PossibleResponseId))]
        [InverseProperty(nameof(PossibleResponses.Responses))]
        public virtual PossibleResponses PossibleResponse { get; set; }
        [ForeignKey(nameof(QuestionExecutionId))]
        [InverseProperty(nameof(QuestionExecutions.Responses))]
        public virtual QuestionExecutions QuestionExecution { get; set; }
    }
}
