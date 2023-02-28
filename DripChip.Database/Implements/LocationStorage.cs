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

        public async Task<LocationViewModel> CreateLocationAsync(CreateLocationContract contract)
        {
            var location = _mapper.Map<Location>(contract);
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

            return _mapper.Map<LocationViewModel>(contract);
        }

        public async Task<LocationViewModel> UpdateLocationAsync(UpdateLocationContract contract)
        {
            var location = await _context.Locations.Where(el =>
                el.Id == contract.Id).FirstOrDefaultAsync();
            if (location == null)
            {
                return null;
            }
            _mapper.Map(contract, location);

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

        public async Task<bool> IsPointExist(double latitude, double longitude)
        {
            var location = await _context.Locations.Where(el =>
              el.Latitude == latitude && el.Longitude == longitude).FirstOrDefaultAsync();

            return location != null;
        }
    }
}
