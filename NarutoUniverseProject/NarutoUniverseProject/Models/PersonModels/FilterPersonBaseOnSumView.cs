using NarutoUniverseProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class FilterPersonBaseOnSumView
    {
        public IList<Item> Styles { get; set; }
        public IList<Item> Positions { get; set; }
        public IList<Item> Countries { get; set; }
        public IList<Item> PowerSources { get; set; }
    }
}
