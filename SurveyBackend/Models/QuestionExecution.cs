using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public partial class QuestionExecution
    {
        public QuestionExecution()
        {
            Responses = new HashSet<Response>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Position { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Body { get; set; }
        public int ExecutionId { get; set; }
        public int QuestionId { get; set; }

        [ForeignKey(nameof(ExecutionId))]
        [InverseProperty(nameof(Models.Execution.QuestionExecutions))]
        public virtual Execution Execution { get; set; }
        [ForeignKey(nameof(QuestionId))]
        [InverseProperty(nameof(Models.Question.QuestionExecutions))]
        public virtual Question Question { get; set; }
        [InverseProperty("QuestionExecution")]
        public virtual ICollection<Response> Responses { get; set; }
    }
}
