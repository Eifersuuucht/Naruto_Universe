using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.AbilityModels
{
    public class EditAbilityBase
    {
        [Required]
        [Display(Name = "Name:")]
        public String Name { get; set; }
        [Display(Name = "Time To Cast (seconds):")]
        public Int32 TimeToCast { get; set; }
        [Display(Name = "Style:")]
        public Int32 StyleId { get; set; }
    }
}
