using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class GetFilteredAnimalVisitedLocationContract : CommonFilterContract
    {
        [FromRoute]
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [FromQuery]
        public DateTime? StartDateTime { get; set; }

        [FromQuery]
        public DateTime? EndDateTime { get; set; }
    }
}
