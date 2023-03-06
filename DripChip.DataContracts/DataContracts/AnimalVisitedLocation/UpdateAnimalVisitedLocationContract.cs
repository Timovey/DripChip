using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class UpdateAnimalVisitedLocationContract : CommonBodyContract<AnimalVisitedLocationBody>
    {
        [FromRoute]
        [GreaterThanZero]
        [ModelBinder(Name = "animalId")]
        public long AnimalId { get; set; }
    }
}
