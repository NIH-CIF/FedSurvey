using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FedSurvey.Models
{
    public partial class QuestionTypeString
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        public bool Preferred { get; set; }

        [ForeignKey(nameof(QuestionTypeId))]
        [InverseProperty(nameof(Models.QuestionType.QuestionTypeStrings))]
        public virtual QuestionType QuestionType { get; set; }
    }
}
