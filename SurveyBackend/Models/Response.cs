using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public partial class Response
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Count { get; set; }
        public int QuestionExecutionId { get; set; }
        public int PossibleResponseId { get; set; }
        public int DataGroupId { get; set; }

        [ForeignKey(nameof(DataGroupId))]
        [InverseProperty(nameof(Models.DataGroup.Responses))]
        public virtual DataGroup DataGroup { get; set; }
        [ForeignKey(nameof(PossibleResponseId))]
        [InverseProperty(nameof(Models.PossibleResponse.Responses))]
        public virtual PossibleResponse PossibleResponse { get; set; }
        [ForeignKey(nameof(QuestionExecutionId))]
        [InverseProperty(nameof(Models.QuestionExecution.Responses))]
        public virtual QuestionExecution QuestionExecution { get; set; }
    }
}
