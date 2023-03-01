using DripChip.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database
{
    public class DripChipContext : DbContext
    {
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Location> Locations { get; set; }
        internal DbSet<AnimalType> AnimalTypes { get; set; }
        internal DbSet<Animal> Animals { get; set; }
        internal DbSet<AnimalVisitedLocation> AnimalVisitedLocations { get; set; }

        public DripChipContext(DbContextOptions<DripChipContext> options)
        : base(options)
        {
        }

    }
}
