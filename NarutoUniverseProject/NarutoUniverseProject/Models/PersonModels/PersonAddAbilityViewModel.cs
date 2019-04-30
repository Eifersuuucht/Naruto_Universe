using NarutoUniverseProject.Models.AbilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class PersonAddAbilityViewModel
    {
        public Int32 Id { get; set; }
        public ICollection<AbilitySummaryViewModel> AbilitiesForAdding { get; set; }
    }
}
