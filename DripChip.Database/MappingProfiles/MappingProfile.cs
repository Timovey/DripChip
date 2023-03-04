using AutoMapper;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.DataContracts.DataContracts.AnimalVisitedLocation;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.DataContracts.DataContracts.Location;
using DripChip.DataContracts.ViewModels;

namespace DripChip.Database.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<Account, AccountViewModel>();
            CreateMap<AccountBody, Account>();

            CreateMap<LocationBody, Location>();
            CreateMap<Location, LocationViewModel>();

            CreateMap<AnimalTypeBody, AnimalType>();
            CreateMap<AnimalType, AnimalTypeViewModel>();

            CreateMap<CreateAnimalContract, Animal>();
            CreateMap<AnimalBody, Animal>();
            CreateMap<Animal, AnimalViewModel>();

            CreateMap<CreateAnimalVisitedLocationContract, AnimalVisitedLocation>();
            CreateMap<UpdateAnimalVisitedLocationContract, AnimalVisitedLocation>();
            CreateMap<AnimalVisitedLocation, AnimalVisitedLocationViewModel>();
        }
    }
}
