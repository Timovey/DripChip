using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class CreateAnimalVisitedLocationContract
    {
        public DateTime DateTimeOfVisitLocationPoint { get; } = DateTime.Now;

        [FromRoute]
        [GreaterThanZero]
        [JsonPropertyName("pointId")]
        public long LocationPointId { get; set; }

        [FromRoute]
        [GreaterThanZero]
        [JsonPropertyName("animalId")]
        public long AnimalId { get; set; }
    }
}
