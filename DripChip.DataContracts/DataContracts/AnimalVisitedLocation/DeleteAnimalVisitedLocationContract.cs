using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class DeleteAnimalVisitedLocationContract
    {
        [FromRoute]
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [FromRoute]
        [GreaterThanZero]
        public long VisitedPointId { get; set; }
    }
}
