using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.AnimalType;
using DripChip.Main.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Main.Controllers
{
    [Route("animals/types")]
    [ApiController]
    [Authorize]
    public class AnimalTypeController : ControllerBase
    {
        private IAnimalTypeStorage _storage;
        public AnimalTypeController(IAnimalTypeStorage storage)
        {
            _storage = storage;
        }

        [HttpGet("{typeId}")]
        public async Task<IResult> GetAnimalTypeAsync([FromRoute] long typeId)
        {
            //if (typeId == null || typeId <= 0)
            //{
            //    return Results.BadRequest();
            //}
            try
            {
                var result = await _storage.GetAnimalTypeAsync(typeId);
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
        public async Task<IResult> CreateAnimalTypeAsync(CreateAnimalTypeContract contract)
        {
            //ПРОВЕРКА НА ПРОБЕЛЫ
            if (await _storage.IsAnimalTypeExist(contract.Type))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _storage.CreateAnimalTypeAsync(contract);
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
            //ПРОБЕЛЫ
            //if (contract.Id <= 0)
            //{
            //    return Results.BadRequest();
            //}
            try
            {
                var result = await _storage.UpdateAnimalTypeAsync(contract);
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
            //if (typeId == null || typeId <= 0)
            //{
            //    return Results.BadRequest();
            //}
            try
            {
                var result = await _storage.DeleteAnimalTypeAsync(typeId);
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
