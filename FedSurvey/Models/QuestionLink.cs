using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    // Because of writing in SQL, this is only allowed to go one deep.
    public partial class QuestionLink : Timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuestionGroupId { get; set; }
        public int QuestionId { get; set; }

        [ForeignKey(nameof(QuestionGroupId))]
        [InverseProperty(nameof(Models.QuestionGroup.QuestionLinks))]
        public virtual QuestionGroup QuestionGroup { get; set; }

        [ForeignKey(nameof(QuestionId))]
        [InverseProperty(nameof(Models.Question.QuestionLinks))]
        public virtual Question Question { get; set; }
    }
}
