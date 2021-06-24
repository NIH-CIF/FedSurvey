using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBackend.Models
{
    public partial class QuestionExecutions
    {
        public QuestionExecutions()
        {
            Responses = new HashSet<Responses>();
        }

        [Key]
        public int Id { get; set; }
        public int Position { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Body { get; set; }
        public int ExecutionId { get; set; }
        public int QuestionId { get; set; }

        [ForeignKey(nameof(ExecutionId))]
        [InverseProperty(nameof(Executions.QuestionExecutions))]
        public virtual Executions Execution { get; set; }
        [ForeignKey(nameof(QuestionId))]
        [InverseProperty(nameof(Questions.QuestionExecutions))]
        public virtual Questions Question { get; set; }
        [InverseProperty("QuestionExecution")]
        public virtual ICollection<Responses> Responses { get; set; }
    }
}
