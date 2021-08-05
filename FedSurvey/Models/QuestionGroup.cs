using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class QuestionGroup : Timestamps
    {
        public QuestionGroup()
        {
            QuestionLinks = new HashSet<QuestionLink>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("QuestionGroup")]
        public virtual ICollection<QuestionLink> QuestionLinks { get; set; }
    }
}
