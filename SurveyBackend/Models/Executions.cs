using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class Executions
    {
        public Executions()
        {
            QuestionExecutions = new HashSet<QuestionExecutions>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Key { get; set; }
        [Column(TypeName = "text")]
        public string Notes { get; set; }

        [InverseProperty("Execution")]
        public virtual ICollection<QuestionExecutions> QuestionExecutions { get; set; }
    }
}
