using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class PossibleResponseString : Timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PossibleResponseId { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        public bool Preferred { get; set; }

        [ForeignKey(nameof(PossibleResponseId))]
        [InverseProperty(nameof(Models.PossibleResponse.PossibleResponseStrings))]
        public virtual PossibleResponse PossibleResponse { get; set; }
    }
}
