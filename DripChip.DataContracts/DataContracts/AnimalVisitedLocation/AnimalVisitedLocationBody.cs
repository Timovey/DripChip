using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DripChip.DataContracts.DataContracts.AnimalVisitedLocation
{
    public class AnimalVisitedLocationBody
    {
        [GreaterThanZero]
        public long VisitedLocationPointId { get; set; }

        [FromBody]
        public DateTime DateTimeOfVisitLocationPoint { get; } = DateTime.UtcNow;

        [GreaterThanZero]
        public long LocationPointId { get; set; }
    }
}
