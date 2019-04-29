using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Models.PersonModels
{
    public class EditTopicBase
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public Int32 Age { get; set; }
        public Int32 PositionId { get; set; }
        public Int32 CountryId { get; set; }
    }
}
