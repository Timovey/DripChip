using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.Location
{
    public class LocationBody
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}
