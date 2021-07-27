using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class DataGroup : Timestamps
    {
        public DataGroup()
        {
            Responses = new HashSet<Response>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("DataGroup")]
        public virtual ICollection<Response> Responses { get; set; }

        [InverseProperty("DataGroup")]
        public virtual ICollection<DataGroupString> DataGroupStrings { get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<DataGroupLink> ParentLinks { get; set; }

        [InverseProperty("Child")]
        public virtual ICollection<DataGroupLink> ChildLinks { get; set; }

        public class DTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public static DTO ToDTO(DataGroup dataGroup)
        {
            string name = dataGroup.DataGroupStrings.Count > 0 ? dataGroup.DataGroupStrings.Where(dgs => dgs.Preferred).FirstOrDefault().Name : null;

            return new DTO
            {
                Id = dataGroup.Id,
                Name = name
            };
        }
    }
}
