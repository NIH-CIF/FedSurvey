using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class Token : Timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public string Body { get; set; }
        public DateTime ExpiresAt { get; set; }

        public class DTO
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
        
        public static DTO ToDTO(Token token)
        {
            return new DTO
            {
                Id = token.Id,
                Body = token.Body,
                ExpiresAt = token.ExpiresAt
            };
        }
    }
}
