using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class RemoveTypeInAnimalContract
    {
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [GreaterThanZero]
        public long TypeId { get; set; }
    }
}
