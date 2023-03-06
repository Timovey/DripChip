using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using DripChip.DataContracts.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class GetFilteredAnimalContract  : CommonFilterContract
    {
        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        [GreaterThanZeroOrNull]
        public int? ChipperId { get; set; }

        [GreaterThanZeroOrNull]
        public long? ChippingLocationId { get; set; }

        public GenderType? Gender { get; set; }

        public LifeStatusType? LifeStatus { get; set; }
    }
}
