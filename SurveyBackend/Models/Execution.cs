using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public partial class Execution
    {
        public Execution()
        {
            QuestionExecutions = new HashSet<QuestionExecution>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Key { get; set; }
        [Column(TypeName = "text")]
        public string Notes { get; set; }

        [InverseProperty("Execution")]
        public virtual ICollection<QuestionExecution> QuestionExecutions { get; set; }

        public class DTO
        {
            public int Id { get; set; }
            public string Key { get; set; }
            public string Notes { get; set; }
        }

        public static DTO ToDTO(Execution execution)
        {
            return new DTO
            {
                Id = execution.Id,
                Key = execution.Key,
                Notes = execution.Notes
            };
        }
    }
}
