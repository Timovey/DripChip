using System.ComponentModel.DataAnnotations;

namespace DripChip.Database.Models
{
    public class Location
    {
        [Key]
        public long Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
