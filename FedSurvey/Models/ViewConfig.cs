using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class ViewConfig : Timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ViewId { get; set; }
        [Required]
        [StringLength(300)]
        public string VariableName { get; set; }
        [Required]
        [StringLength(300)]
        public string VariableValue { get; set; }

        [ForeignKey(nameof(ViewId))]
        [InverseProperty(nameof(Models.View.ViewConfigs))]
        public virtual View View { get; set; }
    }
}
