using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class CreateAnimalVisitedLocationContract
    {
        public DateTime DateTimeOfVisitLocationPoint { get; } = DateTime.UtcNow;

        [GreaterThanZero]
        [ModelBinder(Name ="pointId")]
        public long LocationPointId { get; set; }

        [GreaterThanZero]
        [ModelBinder(Name = "animalId")]
        public long AnimalId { get; set; }
    }
}
