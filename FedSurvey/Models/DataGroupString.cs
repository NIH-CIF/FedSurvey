using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FedSurvey.Models
{
    public partial class DataGroupString
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DataGroupId { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        public bool Preferred { get; set; }

        [ForeignKey(nameof(DataGroupId))]
        [InverseProperty(nameof(Models.DataGroup.DataGroupStrings))]
        public virtual DataGroup DataGroup { get; set; }
    }
}
