using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class GetFilteredAnimalContract
    {
        [FromQuery]
        public DateTime? StartDateTime { get; set; }

        [FromQuery]
        public DateTime? EndDateTime { get; set; }

        [FromQuery]
        [GreaterThanZeroOrNull]
        public int? ChipperId { get; set; }

        [FromQuery]
        [GreaterThanZeroOrNull]
        public long? ChippingLocationId { get; set; }

        [FromQuery]
        public GenderType? Gender { get; set; }

        [FromQuery]
        public LifeStatusType? LifeStatus { get; set; }

        [FromQuery]
        [Positive]
        public int From { get; set; }

        [FromQuery]
        [GreaterThanZero]
        public int Size { get; set; } = 10;
    }
}
