using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.Location
{
    public class UpdateLocationContract
    {
        [FromRoute]
        [GreaterThanZero]
        [JsonPropertyName("pointId")]
        public long Id { get; set; }

        [Required]
        [FromBody]
        public double Latitude { get; set; }

        [Required]
        [FromBody]
        public double Longitude { get; set; }
    }
}
