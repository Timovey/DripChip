using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class ChangeTypeInAnimalContract
    {
        [FromRoute]
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [FromBody]
        [GreaterThanZero]
        public long OldTypeId { get; set; }

        [FromBody]
        [GreaterThanZero]
        public long NewTypeId { get; set; }
    }
}
