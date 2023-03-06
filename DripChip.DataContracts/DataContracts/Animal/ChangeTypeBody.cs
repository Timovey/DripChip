using DripChip.DataContracts.Attributes;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class ChangeTypeBody
    {
        [GreaterThanZero]
        public long OldTypeId { get; set; }

        [GreaterThanZero]
        public long NewTypeId { get; set; }
    }
}
