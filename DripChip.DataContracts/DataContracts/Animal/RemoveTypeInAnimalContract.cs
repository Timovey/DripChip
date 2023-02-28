﻿using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class RemoveTypeInAnimalContract
    {
        [FromRoute]
        [GreaterThanZero]
        public long AnimalId { get; set; }

        [FromRoute]
        [GreaterThanZero]
        public long TypeId { get; set; }
    }
}