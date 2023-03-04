using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DripChip.DataContracts.DataContracts.AnimalType
{
    public class AnimalTypeBody
    {
        [Required]
        public string Type { get; set; }
    }
}
