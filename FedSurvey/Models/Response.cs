using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class Response : Timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // 19 total precision to allow 7 digits before the decimal (government wide employees has 6 digits currently,
        // so assuming the government will not start employing 10s of millions of Americans!)
        [Column(TypeName = "decimal(19, 12)")]
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
