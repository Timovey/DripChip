using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.DataContracts.Enums;
using DripChip.Main.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Main.Controllers
{
    [Route("animals")]
    [ApiController]
    [Authorize]
    public class AnimalController : ControllerBase
    {
        private IAnimalTypeStorage _animalTypeStorage;
        private IAccountStorage _accountStorage;
        private IAnimalStorage _animalStorage;
        private ILocationStorage _locationStorage;
        public AnimalController(IAnimalTypeStorage animalTypeStorage, 
            IAnimalStorage animalStorage,
            IAccountStorage accountStorage,
            ILocationStorage locationStorage)
        {
            _animalStorage = animalStorage;
            _animalTypeStorage = animalTypeStorage;
            _accountStorage = accountStorage;
            _locationStorage = locationStorage;
        }
        private bool IsAnimalTypesValid(long[] animalTypes)
        {
            if (animalTypes == null || animalTypes.Count() <= 0)
            {
                return false;
            }
            foreach (var type in animalTypes)
            {
                if (type == 0)  return false;
            }
            return true;
        }

        [HttpGet("{animalId}")]
        public async Task<IResult> GetAnimalAsync([FromRoute] int animalId)
        {
            try
            {
                var result = await _animalStorage.GetAnimalAsync(animalId);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpGet("/search")]
        public async Task<IResult> GetFilteredAccountAsync(GetFilteredAnimalContract contract)
        {
            //ФОРМАТ ДАТЫ
            try
            {
                var result = await _animalStorage.GetFilteredAnimalAsync(contract);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IResult> CreateAnimalAsync(CreateAnimalContract contract)
        {
            if (!IsAnimalTypesValid(contract.AnimalTypes))
            {
                return Results.BadRequest();
            }
            if(contract.AnimalTypes.Distinct().Count() != contract.AnimalTypes.Count())
            {
                return Results.Conflict();
            }
                foreach (var type in contract.AnimalTypes)
            {
                if(await _animalTypeStorage.GetAnimalTypeAsync(type) == null)
                {
                    return Results.NotFound();
                }
            }
            if(await _accountStorage.GetAccountAsync(contract.ChipperId) == null)
            {
                return Results.NotFound();
            }
            if (await _locationStorage.GetLocationAsync(contract.ChippingLocationId) == null)
            {
                return Results.NotFound();
            }
            try
            {
                var result = await _animalStorage.CreateAnimalAsync(contract);
                return Results.Created(HttpContext.Request.Path, result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        //Новая точка чипирования совпадает с первой посещенной точкой локации
        [HttpPut("{animalId}")]
        public async Task<IResult> UpdateAnimalAsync(UpdateAnimalContract contract)
        {
            var animal = await _animalStorage.GetAnimalAsync(contract.Id);
            if (animal == null)
            {
                return Results.NotFound();
            }
            if (await _accountStorage.GetAccountAsync(contract.ChipperId) == null)
            {
                return Results.NotFound();
            }
            if (await _locationStorage.GetLocationAsync(contract.ChippingLocationId) == null)
            {
                return Results.NotFound();
            }
            if(animal.LifeStatus == LifeStatusType.DEAD 
                && contract.LifeStatus== LifeStatusType.ALIVE)
            {
                 return Results.BadRequest();
            }
            try
            {
                var result = await _animalStorage.UpdateAnimalAsync(contract);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        //Животное покинуло локацию чипирования, при этом
        //есть другие посещенные точки

        [HttpDelete("{animalId}")]
        public async Task<IResult> DeleteAnimalAsync([FromRoute] int animalId)
        {
            try
            {
                var result = await _animalStorage.DeleteAnimalAsync(animalId);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPost("{animalId}/types/{typeId}")]
        public async Task<IResult> AddTypeToAnimalAsync(AddTypeToAnimalContract contract)
        {
            if (await _animalStorage.GetAnimalAsync(contract.AnimalId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.TypeId) == null) { 
                return Results.NotFound();
            }
            try
            {
                var result = await _animalStorage.AddTypeToAnimalAsync(contract);
                if (result == null)
                {
                    return Results.Conflict();
                }
                return Results.Created(HttpContext.Request.Path, result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("{animalId}/types")]
        public async Task<IResult> ChangeTypeInAnimalAsync(ChangeTypeInAnimalContract contract)
        {
            if (await _animalStorage.GetAnimalAsync(contract.AnimalId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.OldTypeId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.NewTypeId) == null)
            {
                return Results.NotFound();
            }
            if(await _animalStorage.IsAnimalTypeExistInAnimal(contract.AnimalId, contract.OldTypeId))
            {
                return Results.NotFound();
            }
            try
            {
                var result = await _animalStorage.ChangeTypeInAnimalAsync(contract);
                if (result == null)
                {
                    return Results.Conflict();
                }
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpDelete("{animalId}/types/{typeId}")]
        public async Task<IResult> RemoveTypeInAnimalAsync(RemoveTypeInAnimalContract contract)
        {
            if (await _animalStorage.GetAnimalAsync(contract.AnimalId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.TypeId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalStorage.IsAnimalTypeExistInAnimal(contract.AnimalId, contract.TypeId))
            {
                return Results.NotFound();
            }
            try
            {
                var result = await _animalStorage.RemoveTypeInAnimalAsync(contract);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
