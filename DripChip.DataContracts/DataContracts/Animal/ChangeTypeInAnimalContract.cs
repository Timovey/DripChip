using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class ChangeTypeInAnimalContract : CommonBodyContract<ChangeTypeBody>
    {
        [GreaterThanZero]
        public long AnimalId { get; set; }
    }
}
