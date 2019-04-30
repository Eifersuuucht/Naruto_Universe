using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class FilterPersonBaseOnSumView
    {
        public IDictionary<String, Boolean> Styles { get; set; }
        public IDictionary<String, Boolean> Positions { get; set; }
        public IDictionary<String, Boolean> Countries { get; set; }
        public IDictionary<String, Boolean> PowerSources { get; set; }
    }
}
