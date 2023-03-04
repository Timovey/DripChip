using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class UpdateAnimalVisitedLocationContract
    {
        [FromBody]
        [GreaterThanZero]
        [JsonPropertyName("visitedLocationPointId")]
        public long Id { get; set; }

        [FromBody]
        public DateTime DateTimeOfVisitLocationPoint { get; } = DateTime.UtcNow;

        [FromBody]
        [GreaterThanZero]
        [JsonPropertyName("pointId")]
        public long LocationPointId { get; set; }

        [FromRoute]
        [GreaterThanZero]
        [JsonPropertyName("animalId")]
        public long AnimalId { get; set; }
    }
}
