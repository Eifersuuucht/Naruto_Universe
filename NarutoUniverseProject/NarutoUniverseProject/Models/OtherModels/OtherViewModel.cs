using NarutoUniverseProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.OtherModels
{
    public class OtherViewModel
    {
        public ICollection<Other> Positions { get; set; }
        public ICollection<Other> Countries { get; set; }
        public ICollection<Style> Styles { get; set; }
        public ICollection<Other> PowerSources { get; set; }

    }
}
