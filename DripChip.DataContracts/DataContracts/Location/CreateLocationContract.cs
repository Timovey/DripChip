using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.Location
{
    public class CreateLocationContract
    {
        [Required]
        [FromBody]
        public double Latitude { get; set; }

        [Required]
        [FromBody]
        public double Longitude { get; set; }
    }
}
