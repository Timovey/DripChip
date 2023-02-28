﻿using DripChip.DataContracts.DataContracts.Location;
using DripChip.DataContracts.ViewModels;

namespace DripChip.Database.Interfaces
{
    public interface ILocationStorage
    {
        public Task<LocationViewModel> CreateLocationAsync(CreateLocationContract contract);

        public Task<LocationViewModel> UpdateLocationAsync(UpdateLocationContract contract);

        public Task<LocationViewModel> GetLocationAsync(long pointId);

        public Task<bool> DeleteLocationAsync(long pointId);

        public Task<bool> IsPointExist(double latitude, double longitude);
    }
}
