using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.AnimalType
{
    public class UpdateAnimalTypeContract
    {
        [FromRoute]
        [Required]
        [GreaterThanZero]
        [JsonPropertyName("typeId")]
        public long Id { get; set; }

        [FromBody]
        [Required]
        public string Type { get; set; }
    }
}
