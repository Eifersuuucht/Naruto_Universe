using NarutoUniverseProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.AbilityModels
{
    public class FilterAbilityBaseOnSumView : FilterBaseOnSumView
    {
        [Display(Name = "Name: ")]
        public String Name { get; set; }

        [Display(Name = "Min Time To Cast: ")]
        public Int32 MinTimeToCast { get; set; }

        [Display(Name = "Max Time To Cast: ")]
        public Int32 MaxTimeToCast { get; set; }
    }
}
