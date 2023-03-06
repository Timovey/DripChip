using DripChip.DataContracts.Attributes;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class AddTypeToAnimalContract
    {
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [GreaterThanZero]
        public long TypeId { get; set; }
    }
}
