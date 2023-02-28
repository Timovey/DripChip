using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database.Interfaces
{
    public interface IAnimalStorage
    {
        public Task<AnimalViewModel> CreateAnimalAsync(CreateAnimalContract contract);

        public Task<AnimalViewModel> UpdateAnimalAsync(UpdateAnimalContract contract);

        public Task<AnimalViewModel> GetAnimalAsync(long animalId);

        public Task<IList<AnimalViewModel>> GetFilteredAnimalAsync(GetFilteredAnimalContract contract);

        public Task<bool> DeleteAnimalAsync(long animalId);

        public Task<bool> IsAnimalExist(long animalId);

        public Task<AnimalViewModel> AddTypeToAnimalAsync(AddTypeToAnimalContract contract);

        public Task<bool> IsAnimalTypeExistInAnimal(long animalId, long typeId);

        public Task<AnimalViewModel> ChangeTypeInAnimalAsync(ChangeTypeInAnimalContract contract);

        public Task<AnimalViewModel> RemoveTypeInAnimalAsync(RemoveTypeInAnimalContract contract);
    }
}
