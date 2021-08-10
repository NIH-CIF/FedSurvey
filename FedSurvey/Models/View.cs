using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using FedSurvey.Models.Util;

namespace FedSurvey.Models
{
    public partial class View : Timestamps
    {
        public View()
        {
            ViewConfigs = new HashSet<ViewConfig>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("View")]
        public virtual ICollection<ViewConfig> ViewConfigs { get; set; }

        public class DTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Config { get; set; }
        }

        public static DTO ToDTO(View view)
        {
            var configObject = new ExpandoObject() as IDictionary<string, object>;
            List<string> variableNames = view.ViewConfigs.Select(vc => vc.VariableName.Split('.')[0]).Distinct().ToList();

            foreach (string variableName in variableNames)
            {
                List<ViewConfig> configs = view.ViewConfigs.Where(vc => vc.VariableName.StartsWith(variableName)).ToList();

                if (configs.Count == 1 && !variableName.EndsWith('s') && !configs[0].VariableName.Contains('.'))
                {
                    configObject.Add(configs[0].VariableName, configs[0].VariableValue);
                }
                else
                {
                    configs = view.ViewConfigs.Where(vc => vc.VariableName.StartsWith(variableName + '.')).ToList();
                    var innerExpando = new ExpandoObject() as IDictionary<string, object>;

                    foreach (ViewConfig config in configs)
                    {
                        string[] parts = config.VariableName.Split('.');

                        if (!innerExpando.ContainsKey(parts.Last()))
                        {
                            List<ViewConfig> innerConfigs = view.ViewConfigs.Where(vc => vc.VariableName.Equals(config.VariableName)).ToList();

                            if (innerConfigs.Count == 1 && !innerConfigs[0].VariableName.EndsWith('s'))
                                innerExpando.Add(parts.Last(), innerConfigs[0].VariableValue);
                            else
                                innerExpando.Add(parts.Last(), innerConfigs.Select(vc => vc.VariableValue).ToArray());
                        }
                    }

                    configObject.Add(variableName, innerExpando);
                }
            }

            return new DTO
            {
                Id = view.Id,
                Name = view.Name,
                Config = Newtonsoft.Json.JsonConvert.SerializeObject(configObject)
            };
        }
    }
}
