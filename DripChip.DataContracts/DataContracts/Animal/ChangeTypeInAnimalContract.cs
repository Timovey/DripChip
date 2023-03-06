using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class ChangeTypeInAnimalContract : CommonBodyContract<ChangeTypeBody>
    {
        [GreaterThanZero]
        public long AnimalId { get; set; }
    }
}
