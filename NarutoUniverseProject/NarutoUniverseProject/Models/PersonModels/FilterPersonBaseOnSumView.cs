using NarutoUniverseProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class FilterPersonBaseOnSumView : FilterBaseOnSumView
    {
        [Display(Name = "Name: ")]
        public String Name { get; set; }

        [Display(Name = "Min Age: ")]
        public Int32 MinAge { get; set; }

        [Display(Name = "Max Age: ")]
        public Int32 MaxAge { get; set; }
    }
}
