using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.AnimalType
{
    public class AnimalTypeBody
    {
        [Required]
        public string Type { get; set; }
    }
}
