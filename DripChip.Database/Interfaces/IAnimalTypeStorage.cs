﻿using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.DataContracts.ViewModels;

namespace DripChip.Database.Interfaces
{
    public interface IAnimalTypeStorage
    {
        public Task<AnimalTypeViewModel> CreateAnimalTypeAsync(AnimalTypeBody contract);

        public Task<AnimalTypeViewModel> UpdateAnimalTypeAsync(UpdateAnimalTypeContract contract);

        public Task<AnimalTypeViewModel> GetAnimalTypeAsync(long animalTypeId);

        public Task<bool> DeleteAnimalTypeAsync(long animalTypeId);

        public Task<bool> IsAnimalTypeExist(string type);

        public Task<bool> IsAnimalHasType(long typeId);
    }
}
