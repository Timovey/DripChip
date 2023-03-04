using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.Main.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Main.Controllers
{
    [Route("animals/types")]
    [ApiController]
    [Authorize]
    public class AnimalTypeController : ControllerBase
    {
        private IAnimalTypeStorage _animalTypeStarage;
        public AnimalTypeController(IAnimalTypeStorage storage)
        {
            _animalTypeStarage = storage;
        }

        [HttpGet("{typeId}")]
        [NotStrict]
        public async Task<IResult> GetAnimalTypeAsync([FromRoute] long typeId)
        {
            if (typeId == null || typeId <= 0)
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _animalTypeStarage.GetAnimalTypeAsync(typeId);
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

        [HttpPost]
        public async Task<IResult> CreateAnimalTypeAsync([FromBody] AnimalTypeBody contract)
        {
            if (await _animalTypeStarage.IsAnimalTypeExist(contract.Type))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _animalTypeStarage.CreateAnimalTypeAsync(contract);
                return Results.Created(HttpContext.Request.Path, result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("{typeId}")]
        public async Task<IResult> UpdateAnimalTypeAsync(UpdateAnimalTypeContract contract)
        {         
            if(await _animalTypeStarage.IsAnimalTypeExist(contract.Body.Type))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _animalTypeStarage.UpdateAnimalTypeAsync(contract);
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

        [HttpDelete("{typeId}")]
        public async Task<IResult> DeleteAnimalTypeAsync([FromRoute] long typeId)
        {
            //ТОЧКА СВЯЗАНА С ЖИВОТНЫМ - проверка
            if (typeId == null || typeId <= 0)
            {
                return Results.BadRequest();
            }
            if(await _animalTypeStarage.IsAnimalHasType(typeId))
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _animalTypeStarage.DeleteAnimalTypeAsync(typeId);
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
    }
}
