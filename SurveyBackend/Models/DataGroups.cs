using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class DataGroups
    {
        public DataGroups()
        {
            Responses = new HashSet<Responses>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("DataGroup")]
        public virtual ICollection<Responses> Responses { get; set; }
    }
}
