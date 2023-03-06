using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalVisitedLocation;
using DripChip.DataContracts.Enums;
using DripChip.Main.Attributes;
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
        private bool IsAnimalTypesValid(List<long> animalTypes)
        {
            if (animalTypes == null || animalTypes.Count() <= 0)
            {
                return false;
            }
            foreach (var type in animalTypes)
            {
                if (type <= 0)  return false;
            }
            return true;
        }

        #region Animal

        [HttpGet("search")]
        [NotStrict]
        public async Task<IResult> GetFilteredAccountAsync([FromQuery] GetFilteredAnimalContract? contract)
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

        [HttpGet("{animalId}")]
        [NotStrict]
        public async Task<IResult> GetAnimalAsync([FromRoute] int animalId)
        {
            if (animalId == null || animalId <= 0)
            {
                return Results.BadRequest();
            }
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

        [HttpPost("")]
        public async Task<IResult> CreateAnimalAsync([FromBody] CreateAnimalContract contract)
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
            var animal = await _animalStorage.GetAnimalAsync(contract.AnimalId);
            if (animal == null)
            {
                return Results.NotFound();
            }
            if (await _accountStorage.GetAccountAsync(contract.Body.ChipperId) == null)
            {
                return Results.NotFound();
            }
            if (await _locationStorage.GetLocationAsync(contract.Body.ChippingLocationId) == null)
            {
                return Results.NotFound();
            }
            if(animal.LifeStatus == LifeStatusType.DEAD
                && contract.Body.LifeStatus == LifeStatusType.ALIVE)
            {
                 return Results.BadRequest();
            }
            if (await _animalStorage.IsFirstPointEqualsChipPoint(contract.AnimalId,
                contract.Body.ChippingLocationId))
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
            if (animalId == null || animalId <= 0)
            {
                return Results.BadRequest();
            }
            var animal = await _animalStorage.GetAnimalAsync(animalId);
            if (animal == null)
            {
                return Results.NotFound();
            }
            if(animal.VisitedLocations.Count() > 0)
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _animalStorage.DeleteAnimalAsync(animalId);
                if (result == null || !result)
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

        #endregion

        #region Animal Type Action

        [HttpPost("{animalId}/types/{typeId}")]
        public async Task<IResult> AddTypeToAnimalAsync([FromRoute] AddTypeToAnimalContract contract)
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
        public async Task<IResult> ChangeTypeInAnimalAsync([FromRoute] ChangeTypeInAnimalContract contract)
        {
            if (await _animalStorage.GetAnimalAsync(contract.AnimalId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.Body.OldTypeId) == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.Body.NewTypeId) == null)
            {
                return Results.NotFound();
            }
            if(!await _animalStorage.IsAnimalTypeExistInAnimal(contract.AnimalId, contract.Body.OldTypeId))
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
        public async Task<IResult> RemoveTypeInAnimalAsync([FromRoute] RemoveTypeInAnimalContract contract)
        {
            var animal = await _animalStorage.GetAnimalAsync(contract.AnimalId);
            if (animal == null)
            {
                return Results.NotFound();
            }
            if (await _animalTypeStorage.GetAnimalTypeAsync(contract.TypeId) == null)
            {
                return Results.NotFound();
            }
            if (!await _animalStorage.IsAnimalTypeExistInAnimal(contract.AnimalId, contract.TypeId))
            {
                return Results.NotFound();
            }
            if(animal.AnimalTypes.Count == 1)
            {
                return Results.BadRequest();
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
        #endregion

        #region Visited Points

        [HttpGet("{animalId}/locations")]
        [NotStrict]
        public async Task<IResult> GetFilteredAmimalVisitedLocationAsync(
           GetFilteredAnimalVisitedLocationContract contract)
        {
            try
            {
                var result = await _animalStorage.GetFilteredAmimalVisitedLocationAsync(contract);

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

        [HttpPost("{animalId}/locations/{pointId}")]
        public async Task<IResult> AddVisitedLocationToAnimalAsync([FromRoute] CreateAnimalVisitedLocationContract contract)
        {
            var animal = await _animalStorage.GetAnimalAsync(contract.AnimalId);
            if(animal == null)
            {
                return Results.NotFound();
            }
            if(animal.LifeStatus == LifeStatusType.DEAD)
            {
                return Results.BadRequest();
            }
            var point = await _locationStorage.GetLocationAsync(contract.LocationPointId);
            if (point == null)
            {
                return Results.NotFound();
            }
            if(animal.ChippingLocationId == contract.LocationPointId)
            {
                return Results.BadRequest();
            }
            if(await _animalStorage.IsLastPointEqualsNewPoint(contract.AnimalId, contract.LocationPointId))
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _animalStorage.AddAnimalVisitedLocationAsync(contract);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Created(HttpContext.Request.Path, result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("{animalId}/locations")]
        public async Task<IResult> UpdateAnimalVisitedLocationAsync(
            UpdateAnimalVisitedLocationContract contract)
        {
            var animal = await _animalStorage.GetAnimalAsync(contract.AnimalId);
            if (animal == null)
            {
                return Results.NotFound();
            }
            if (await _locationStorage.GetLocationAsync(contract.Body.LocationPointId) == null)
            {
                return Results.NotFound();
            }
            var animalVisitedLocation = await _animalStorage.GetAnimalVisitedLocationAsync(contract.Body.VisitedLocationPointId);
            if (animalVisitedLocation == null) 
            {
                return Results.NotFound();
            }
            if(!animal.VisitedLocations.Contains(contract.Body.VisitedLocationPointId)) 
            { 
                return Results.NotFound();
            }


            if (animal.ChippingLocationId == contract.Body.LocationPointId)
            {
                return Results.BadRequest();
            }
            if (animal.VisitedLocations.FirstOrDefault() == contract.Body.VisitedLocationPointId && 
                contract.Body.LocationPointId == animal.ChippingLocationId)
            {
                return Results.BadRequest();
            }
            if(animalVisitedLocation.LocationPointId == contract.Body.LocationPointId)
            {
                return Results.BadRequest();
            }
            for(int i = 0; i < animal.VisitedLocations.Count(); i++)
            {
                if (animal.VisitedLocations[i] == contract.Body.VisitedLocationPointId)
                {
                    if(i - 1 >= 0 && (await _animalStorage
                        .GetAnimalVisitedLocationAsync(animal.VisitedLocations[i - 1])).LocationPointId 
                        == contract.Body.LocationPointId)
                    {
                        return Results.BadRequest();
                    }

                    if (i + 1 < animal.VisitedLocations.Count() && (await _animalStorage
                        .GetAnimalVisitedLocationAsync(animal.VisitedLocations[i + 1])).LocationPointId
                        == contract.Body.LocationPointId)
                    {
                        return Results.BadRequest();
                    }
                    break;
                }
            }
            try
            {
                var result = await _animalStorage.UpdateAnimalVisitedLocationAsync(contract);
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

        [HttpDelete("{animalId}/locations/{visitedPointId}")]
        public async Task<IResult> DeleteAnimalVisitedLocationAsync(DeleteAnimalVisitedLocationContract contract)
        {
            var animal = await _animalStorage.GetAnimalAsync(contract.AnimalId);
            if (animal == null)
            {
                return Results.NotFound();
            }
           
            var animalVisitedLocation = await _animalStorage.GetAnimalVisitedLocationAsync(contract.VisitedPointId);
            if (animalVisitedLocation == null)
            {
                return Results.NotFound();
            }
            if (!animal.VisitedLocations.Contains(contract.VisitedPointId))
            {
                return Results.NotFound();
            }
            try
            {
                var result = await _animalStorage.DeleteAnimalVisitedLocationAsync(contract);
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
        #endregion
    }
}
