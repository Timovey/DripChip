using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.AnimalType
{
    public class CreateAnimalTypeContract
    {
        [FromBody]
        [Required]
        public string Type { get; set; }
    }
}
