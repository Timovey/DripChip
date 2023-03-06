using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class DeleteAnimalVisitedLocationContract
    {
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [GreaterThanZero]
        public long VisitedPointId { get; set; }
    }
}
