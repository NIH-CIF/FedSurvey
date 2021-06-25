using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public partial class PossibleResponseString
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PossibleResponseId { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [ForeignKey(nameof(PossibleResponseId))]
        [InverseProperty(nameof(Models.PossibleResponse.PossibleResponseStrings))]
        public virtual PossibleResponse PossibleResponse { get; set; }
    }
}
