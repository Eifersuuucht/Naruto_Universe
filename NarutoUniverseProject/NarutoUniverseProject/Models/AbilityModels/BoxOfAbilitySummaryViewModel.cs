using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.AbilityModels
{
    public class BoxOfAbilitySummaryViewModel : FilterAbilityBaseOnSumView
    {
        public ICollection<AbilitySummaryViewModel> ViewModels { get; set; }
    }
}
