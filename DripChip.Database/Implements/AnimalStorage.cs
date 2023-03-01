﻿using AutoMapper;
using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalVisitedLocation;
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

            return _mapper.Map<AnimalViewModel>(contract);
        }

        public async Task<AnimalViewModel> UpdateAnimalAsync(UpdateAnimalContract contract)
        {
            var animal = await _context.Animals.Where(el =>
                el.Id == contract.Id).FirstOrDefaultAsync();
            if (animal == null)
            {
                return null;
            }
            _mapper.Map(contract, animal);

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
                .Where(el => contract.StartDateTime != null
                    && el.ChippingDateTime >= contract.StartDateTime)
                .Where(el => contract.EndDateTime != null
                    && el.ChippingDateTime <= contract.EndDateTime)
                .Where(el => contract.ChipperId != null
                    && el.ChipperId == contract.ChipperId)
                .Where(el => contract.ChippingLocationId != null
                    && el.ChippingLocationId == contract.ChippingLocationId)
                .Where(el => contract.LifeStatus.HasValue
                    && el.LifeStatus == (byte)contract.LifeStatus.Value)
                .Where(el => contract.Gender.HasValue
                    && el.Gender == (byte)contract.Gender.Value)
                .Skip(contract.From)
                .Take(contract.Size)
                .OrderBy(el => el.Id).ToListAsync();

            var result = new List<AnimalViewModel>();
            foreach (var animal in animals)
            {
                _mapper.Map<AnimalViewModel>(animal);
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
            animal.AnimalTypes.Append(contract.TypeId);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalViewModel>(contract);
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
            if (animal.AnimalTypes.Contains(contract.NewTypeId))
            {
                return null;
            }
            
            
            for(long i = 0; i < animal.AnimalTypes.Count(); i++)
            {
                if (animal.AnimalTypes[i] == contract.OldTypeId)
                {
                    animal.AnimalTypes[i] = contract.NewTypeId;
                    await _context.SaveChangesAsync();
                    break;
                }
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

            if(animal.AnimalTypes.ToList().Remove(contract.TypeId))
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
            animal.VisitedLocations.Append(animalVisitedLocation.Id);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalVisitedLocationViewModel>(animalVisitedLocation);
        }

        public async Task<AnimalVisitedLocationViewModel> UpdateAnimalVisitedLocationAsync(
            UpdateAnimalVisitedLocationContract contract)
        {
            var animalVisitedLocation = await _context.AnimalVisitedLocations
                .Where(el => el.Id == contract.LocationPointId).FirstOrDefaultAsync();

            if(animalVisitedLocation == null)
            {
                return null;
            }

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
                animal.VisitedLocations.ToList().Remove(contract.VisitedPointId);
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
    }
}
