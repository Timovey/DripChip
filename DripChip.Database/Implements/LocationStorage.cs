using AutoMapper;
using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Location;
using DripChip.DataContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database.Implements
{
    public class LocationStorage : ILocationStorage
    {
        private readonly DripChipContext _context;
        private readonly IMapper _mapper;
        public LocationStorage(
            DripChipContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LocationViewModel> CreateLocationAsync(LocationBody contract)
        {
            var location = _mapper.Map<Location>(contract);
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

            return _mapper.Map<LocationViewModel>(location);
        }

        public async Task<LocationViewModel> UpdateLocationAsync(UpdateLocationContract contract)
        {
            var location = await _context.Locations.Where(el =>
                el.Id == contract.PointId).FirstOrDefaultAsync();
            if (location == null)
            {
                return null;
            }
            _mapper.Map(contract.Body, location);

            await _context.SaveChangesAsync();

            return _mapper.Map<LocationViewModel>(location);
        }

        public async Task<LocationViewModel> GetLocationAsync(long pointId)
        {
            var location = await _context.Locations.Where(el =>
                el.Id == pointId).FirstOrDefaultAsync();
            return location == null ? null : _mapper.Map<LocationViewModel>(location);
        }  

        public async Task<bool> DeleteLocationAsync(long pointId)
        {
            var location = await _context.Locations.Where(el =>
                el.Id == pointId).FirstOrDefaultAsync();
            if (location == null)
            {
                return false;
            }
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsPointExist(LocationBody contract)
        {
            var location = await _context.Locations.Where(el =>
              el.Latitude == contract.Latitude && el.Longitude == contract.Longitude)
                .FirstOrDefaultAsync();

            return location != null;
        }

        public async Task<bool> IsAnimalHasPoint(long pointId)
        {
            var animals = await _context.Animals.Where(el => 
                el.ChippingLocationId == pointId)
                    .ToListAsync();
            if(animals != null && animals.Count> 0)
            {
                return true;
            }
            var visitedLocations = await _context.AnimalVisitedLocations.Where(el =>
                el.LocationPointId == pointId).ToListAsync();
            if(visitedLocations != null && visitedLocations.Count> 0)
            {
                return true;
            }
            return false;
        }
    }
}
