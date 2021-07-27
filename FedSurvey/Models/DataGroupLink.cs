using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    // Because of writing in SQL, this is only allowed to go one deep.
    public partial class DataGroupLink : Timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Models.DataGroup.ParentLinks))]
        public virtual DataGroup Parent { get; set; }

        [ForeignKey(nameof(ChildId))]
        [InverseProperty(nameof(Models.DataGroup.ChildLinks))]
        public virtual DataGroup Child { get; set; }
    }
}
