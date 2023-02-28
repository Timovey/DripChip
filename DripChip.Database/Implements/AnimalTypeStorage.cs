using AutoMapper;
using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.DataContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database.Implements
{
    public class AnimalTypeStorage  : IAnimalTypeStorage
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

        public async Task<AnimalTypeViewModel> CreateAnimalTypeAsync(CreateAnimalTypeContract contract)
        {
            var animalType = _mapper.Map<AnimalType>(contract);
            await _context.AnimalTypes.AddAsync(animalType);
            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalTypeViewModel>(contract);
        }

        public async Task<AnimalTypeViewModel> UpdateAnimalTypeAsync(UpdateAnimalTypeContract contract)
        {
            var animalType = await _context.AnimalTypes.Where(el =>
                el.Id == contract.Id).FirstOrDefaultAsync();
            if (animalType == null)
            {
                return null;
            }
            _mapper.Map(contract, animalType);

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
    }
}
