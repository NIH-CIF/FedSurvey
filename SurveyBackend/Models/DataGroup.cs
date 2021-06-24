using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public partial class DataGroup
    {
        public DataGroup()
        {
            Responses = new HashSet<Response>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("DataGroup")]
        public virtual ICollection<Response> Responses { get; set; }
    }
}
