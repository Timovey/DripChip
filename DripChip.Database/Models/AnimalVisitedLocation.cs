namespace DripChip.Database.Models
{
    public class AnimalVisitedLocation
    {
        public long Id { get; set; }

        public DateTime DateTimeOfVisitLocationPoint { get; set; }

        public long LocationPointId { get; set; }

        public long AnimalId { get; set; }
    }
}
