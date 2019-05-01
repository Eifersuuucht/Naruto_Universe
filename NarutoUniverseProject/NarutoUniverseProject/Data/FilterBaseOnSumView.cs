using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Data
{

    public class FilterBaseOnSumView
    {
        public Boolean Descending { get; set; }
        public String Sorting { get; set; }
        public IList<Item> Styles { get; set; }
        public IList<Item> Positions { get; set; }
        public IList<Item> Countries { get; set; }
        public IList<Item> PowerSources { get; set; }
    }
}
