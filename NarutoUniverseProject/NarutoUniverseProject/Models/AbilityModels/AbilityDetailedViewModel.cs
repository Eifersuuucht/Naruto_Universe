using NarutoUniverseProject.Models.PersonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NarutoUniverseProject.Models.AbilityModels
{
    public class AbilityDetailedViewModel
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Int32 TimeToCast { get; set; }
        public String Style { get; set; }
        public String PowerSource { get; set; }
        public ICollection<PersonSummaryViewModel> Persons { get; set; }
    }
}
