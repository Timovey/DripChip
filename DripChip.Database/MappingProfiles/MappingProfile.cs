using AutoMapper;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.DataContracts.DataContracts.Location;
using DripChip.DataContracts.ViewModels;

namespace DripChip.Database.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<CreateAccountContract, Account>();
            CreateMap<UpdateAccountContract, Account>();
            CreateMap<Account, AccountViewModel>();

            CreateMap<CreateLocationContract, Location>();
            CreateMap<UpdateLocationContract, Location>();
            CreateMap<Location, LocationViewModel>();

            CreateMap<CreateAnimalTypeContract, AnimalType>();
            CreateMap<UpdateAnimalTypeContract, AnimalType>();
            CreateMap<AnimalType, AnimalTypeViewModel>();

            CreateMap<CreateAnimalContract, Animal>();
            CreateMap<UpdateAnimalContract, Animal>();
            CreateMap<Animal, AnimalViewModel>();
        }
    }
}
