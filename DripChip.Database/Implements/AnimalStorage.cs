using AutoMapper;
using DripChip.Database.Extensions;
using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalVisitedLocation;
using DripChip.DataContracts.Enums;
using DripChip.DataContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database.Implements
{
    public class AnimalStorage : IAnimalStorage
    {
        private readonly DripChipContext _context;
        private readonly IMapper _mapper;
        public AnimalStorage(
            DripChipContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AnimalViewModel> CreateAnimalAsync(CreateAnimalContract contract)
        {
            var animal = _mapper.Map<Animal>(contract);
            await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalViewModel>(animal);
        }

        public async Task<AnimalViewModel> UpdateAnimalAsync(UpdateAnimalContract contract)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == contract.AnimalId).FirstOrDefaultAsync();
            if (animal == null)
            {
                return null;
            }
            _mapper.Map(contract.Body, animal);

            if(contract.Body.LifeStatus == LifeStatusType.DEAD)
            {
                animal.DeathDateTime = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalViewModel>(animal);
        }

        public async Task<AnimalViewModel> GetAnimalAsync(long animalId)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == animalId).FirstOrDefaultAsync();
            return animal == null ? null : _mapper.Map<AnimalViewModel>(animal);
        }

        public async Task<IList<AnimalViewModel>> GetFilteredAnimalAsync(GetFilteredAnimalContract contract)
        {
            var animals = await _context.Animals
                .WhereIf(contract.StartDateTime != null, el => 
                    el.ChippingDateTime >= contract.StartDateTime)
                .WhereIf(contract.EndDateTime != null, el => 
                    el.ChippingDateTime <= contract.EndDateTime)
                .WhereIf(contract.ChipperId != null, el =>
                    el.ChipperId == contract.ChipperId)
                .WhereIf(contract.ChippingLocationId != null, el => 
                    el.ChippingLocationId == contract.ChippingLocationId)
                .WhereIf(contract.LifeStatus.HasValue, el =>
                    el.LifeStatus == (byte)contract.LifeStatus.Value)
                .WhereIf(contract.Gender.HasValue, el =>
                    el.Gender == (byte)contract.Gender.Value)
                .OrderBy(el => el.Id)
                .Skip(contract.From)
                .Take(contract.Size)
                .ToListAsync();

            var result = new List<AnimalViewModel>();
            foreach (var animal in animals)
            {
                result.Add(_mapper.Map<AnimalViewModel>(animal));
            }
            return result;
        }

        public async Task<bool> DeleteAnimalAsync(long animalId)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == animalId).FirstOrDefaultAsync();
            if (animal == null)
            {
                return false;
            }
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsAnimalExist(long animalId)
        {
            var animal = await _context.Animals.Where(el =>
                 el.Id == animalId).FirstOrDefaultAsync();
            return animal != null;
        }

        public async Task<AnimalViewModel> AddTypeToAnimalAsync(AddTypeToAnimalContract contract)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == contract.AnimalId).FirstOrDefaultAsync();
            if (animal.AnimalTypes.Contains(contract.TypeId))
            {
                return null;
            }
            animal.AnimalTypes.Add(contract.TypeId);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalViewModel>(animal);
        }

        public async Task<bool> IsAnimalTypeExistInAnimal(long animalId, long typeId)
        {
            var animal = await _context.Animals.Where(el =>
                 el.Id == animalId).FirstOrDefaultAsync();
            if(animal == null)
            {
                return false;
            }
            return animal.AnimalTypes.Contains(typeId);
        }

        public async Task<AnimalViewModel> ChangeTypeInAnimalAsync(ChangeTypeInAnimalContract contract)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == contract.AnimalId).FirstOrDefaultAsync();
            if (animal.AnimalTypes.Contains(contract.Body.NewTypeId))
            {
                return null;
            }

            int index = animal.AnimalTypes.FindIndex(el => el == contract.Body.OldTypeId);

            if (index != -1) {
                animal.AnimalTypes[index] = contract.Body.NewTypeId;
            }
            else
            {
                return null;
            }

            return _mapper.Map<AnimalViewModel>(animal);
        }

        public async Task<AnimalViewModel> RemoveTypeInAnimalAsync(RemoveTypeInAnimalContract contract)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == contract.AnimalId).FirstOrDefaultAsync();
            if(animal == null)
            {
                return null;
            }
            if (!animal.AnimalTypes.Contains(contract.TypeId))
            {
                return null;
            }

            if(animal.AnimalTypes.Remove(contract.TypeId))
            {
                await _context.SaveChangesAsync();
                return _mapper.Map<AnimalViewModel>(animal);
            }

            return null;
        }

        public async Task<IList<AnimalVisitedLocationViewModel>> GetFilteredAmimalVisitedLocationAsync(
            GetFilteredAnimalVisitedLocationContract contract)
        {
            var animal = await _context.Animals.Where(el => 
                el.Id == contract.AnimalId).FirstOrDefaultAsync();

            if (animal == null) {
                return null;
            }

            var animalVisitedLocation = await _context.AnimalVisitedLocations
                .Where(el => animal.VisitedLocations.Contains(el.Id))
                .OrderBy(el => el.Id)
                .Skip(contract.From)
                .Take(contract.Size)
                .ToListAsync();

            var result = new List<AnimalVisitedLocationViewModel>();
            foreach(var loc in animalVisitedLocation)
            {
                result.Add(_mapper.Map<AnimalVisitedLocationViewModel>(loc));
            }
            return result;
        }

        public async Task<AnimalVisitedLocationViewModel> GetAnimalVisitedLocationAsync(long id)
        {
            var animalVisitedLocation = await _context.AnimalVisitedLocations
              .Where(el => el.Id == id).FirstOrDefaultAsync();

            if (animalVisitedLocation == null)
            {
                return null;
            }

            return _mapper.Map<AnimalVisitedLocationViewModel>(animalVisitedLocation);
        }

        public async Task<AnimalVisitedLocationViewModel> AddAnimalVisitedLocationAsync(
            CreateAnimalVisitedLocationContract contract)
        {
            var animalVisitedLocation = _mapper.Map<AnimalVisitedLocation>(contract);
                await _context.AnimalVisitedLocations.AddAsync(animalVisitedLocation);

            await _context.SaveChangesAsync();

            var animal = await _context.Animals.Where(el =>
                el.Id == contract.AnimalId).FirstOrDefaultAsync();
            if(animal == null)
            {
                return null;
            }
            animal.VisitedLocations.Add(animalVisitedLocation.Id);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalVisitedLocationViewModel>(animalVisitedLocation);
        }

        public async Task<AnimalVisitedLocationViewModel> UpdateAnimalVisitedLocationAsync(
            UpdateAnimalVisitedLocationContract contract)
        {
            var animalVisitedLocation = await _context.AnimalVisitedLocations
                .Where(el => el.Id == contract.Body.LocationPointId).FirstOrDefaultAsync();


            if(animalVisitedLocation == null)
            {
                return null;
            }
            _mapper.Map(contract.Body, animalVisitedLocation);


            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalVisitedLocationViewModel>(animalVisitedLocation);
        }

        public async Task<bool> DeleteAnimalVisitedLocationAsync(
           DeleteAnimalVisitedLocationContract contract)
        {
            var animal = await _context.Animals.Where(el => 
                el.Id == contract.AnimalId).FirstOrDefaultAsync();

            if(animal == null)
            {
                return false;
            }
            if(animal.VisitedLocations.Contains(contract.VisitedPointId))
            {
                animal.VisitedLocations.Remove(contract.VisitedPointId);
                await _context.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            var animalVisitedLocation = await _context.AnimalVisitedLocations
                .Where(el => el.Id == contract.VisitedPointId).FirstOrDefaultAsync();

            if (animalVisitedLocation == null)
            {
                return false;
            }
            _context.AnimalVisitedLocations.Remove(animalVisitedLocation);
            return true;
        }

        public async Task<bool> IsLastPointEqualsNewPoint(long animalId, long pointId)
        {
            var animal = await _context.Animals.Where(el => el.Id == animalId)
                .FirstOrDefaultAsync();
            var lastVisitedLocationId = animal.VisitedLocations.LastOrDefault();
            if (lastVisitedLocationId == null)
            {
                return false;
            }
            var lastVisitedLocation = await _context.AnimalVisitedLocations.Where(el =>
                el.Id == lastVisitedLocationId).FirstOrDefaultAsync();
            if (lastVisitedLocation == null)
            {
                return false;
            }
            return lastVisitedLocation.LocationPointId == pointId;
        }

        public async Task<bool> IsFirstPointEqualsChipPoint(long animalId, long pointId)
        {
            var animal = await _context.Animals.Where(el => el.Id == animalId)
                .FirstOrDefaultAsync();
            var firstVisitedLocationId = animal.VisitedLocations.FirstOrDefault();
            if (firstVisitedLocationId == null)
            {
                return false;
            }
            var firstVisitedLocation = await _context.AnimalVisitedLocations.Where(el =>
                el.Id == firstVisitedLocationId).FirstOrDefaultAsync();
            if (firstVisitedLocation == null)
            {
                return false;
            }
            return firstVisitedLocation.LocationPointId == pointId;
        }
    }
}
