using NarutoUniverseProject.Models.AbilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class PersonDetailedViewModel
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Int32 Age { get; set; }
        public String Position { get; set; }
        public String Country { get; set; }
        public ICollection<AbilitySummaryViewModel> Abilities { get; set; }
    }
}
