using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.AnimalType
{
    public class UpdateAnimalTypeContract : CommonBodyContract<AnimalTypeBody>
    {
        [FromRoute]
        [GreaterThanZero]
        public long TypeId { get; set; }
    }
}
