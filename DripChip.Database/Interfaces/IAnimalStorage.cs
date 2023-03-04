using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalVisitedLocation;
using DripChip.DataContracts.ViewModels;

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

        public Task<IList<AnimalVisitedLocationViewModel>> GetFilteredAmimalVisitedLocationAsync(
           GetFilteredAnimalVisitedLocationContract contract);

        public Task<AnimalVisitedLocationViewModel> GetAnimalVisitedLocationAsync(long id);

        public Task<AnimalVisitedLocationViewModel> AddAnimalVisitedLocationAsync(
            CreateAnimalVisitedLocationContract contract);

        public Task<AnimalVisitedLocationViewModel> UpdateAnimalVisitedLocationAsync(
            UpdateAnimalVisitedLocationContract contract);

        public Task<bool> DeleteAnimalVisitedLocationAsync(
           DeleteAnimalVisitedLocationContract contract);

        public Task<bool> IsLastPointEqualsNewPoint(long animalId, long pointId);

        public Task<bool> IsFirstPointEqualsChipPoint(long animalId, long pointId);
    }
}
