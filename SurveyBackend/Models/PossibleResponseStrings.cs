using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class PossibleResponseStrings
    {
        [Key]
        public int Id { get; set; }
        public int PossibleResponseId { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [ForeignKey(nameof(PossibleResponseId))]
        [InverseProperty(nameof(PossibleResponses.PossibleResponseStrings))]
        public virtual PossibleResponses PossibleResponse { get; set; }
    }
}
