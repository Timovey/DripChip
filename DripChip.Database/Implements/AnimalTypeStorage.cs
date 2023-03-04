using AutoMapper;
using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.DataContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database.Implements
{
    public class AnimalTypeStorage : IAnimalTypeStorage
    {
        private readonly DripChipContext _context;
        private readonly IMapper _mapper;
        public AnimalTypeStorage(
            DripChipContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AnimalTypeViewModel> CreateAnimalTypeAsync(AnimalTypeBody contract)
        {
            var animalType = _mapper.Map<AnimalType>(contract);
            await _context.AnimalTypes.AddAsync(animalType);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalTypeViewModel>(animalType);
        }

        public async Task<AnimalTypeViewModel> UpdateAnimalTypeAsync(UpdateAnimalTypeContract contract)
        {
            var animalType = await _context.AnimalTypes.Where(el =>
                el.Id == contract.TypeId).FirstOrDefaultAsync();
            if (animalType == null)
            {
                return null;
            }
            _mapper.Map(contract.Body, animalType);

            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalTypeViewModel>(animalType);
        }

        public async Task<AnimalTypeViewModel> GetAnimalTypeAsync(long animalTypeId)
        {
            var animalType = await _context.AnimalTypes.Where(el =>
                el.Id == animalTypeId).FirstOrDefaultAsync();
            return animalType == null ? null : _mapper.Map<AnimalTypeViewModel>(animalType);
        }

        public async Task<bool> DeleteAnimalTypeAsync(long animalTypeId)
        {
            var animalType = await _context.AnimalTypes.Where(el =>
                el.Id == animalTypeId).FirstOrDefaultAsync();
            if (animalType == null)
            {
                return false;
            }
            _context.AnimalTypes.Remove(animalType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsAnimalTypeExist(string type)
        {
            var animalType = await _context.AnimalTypes.Where(el =>
              el.Type == type).FirstOrDefaultAsync();

            return animalType != null;
        }

        public async Task<bool> IsAnimalHasType(long typeId)
        {
            var animals = await _context.Animals.Where(el => 
                el.AnimalTypes.Contains(typeId)).ToListAsync();
            if(animals != null && animals.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
