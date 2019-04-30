using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class EditPersonBase
    {
        [Required]
        [Display(Name = "Name: ")]
        public String Name { get; set; }
        [Required]
        [Display(Name = "Age: ")]
        public Int32 Age { get; set; }
        [Display(Name = "Position: ")]
        public Int32 PositionId { get; set; }
        [Display(Name = "Country: ")]
        public Int32 CountryId { get; set; }
    }
}
