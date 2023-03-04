using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using DripChip.DataContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class UpdateAnimalContract : CommonBodyContract<AnimalBody>
    {
        [FromRoute]
        [GreaterThanZero]
        public long AnimalId { get; set; }
    }
}
