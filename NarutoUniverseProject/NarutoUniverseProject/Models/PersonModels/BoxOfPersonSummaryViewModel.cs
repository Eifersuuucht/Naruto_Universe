using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class BoxOfPersonSummaryViewModel : FilterPersonBaseOnSumView
    {
        public ICollection<PersonSummaryViewModel> ViewModels { get; set; }
    }
}
