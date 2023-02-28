using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.Location;
using DripChip.Main.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Main.Controllers
{
    [Route("[controller]s")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private ILocationStorage _storage;
        public LocationController(ILocationStorage storage)
        {
            _storage = storage;
        }
        private bool IsPointValid(double lalitude, double longitude)
        {
            if (lalitude < -90 || lalitude > 90 ||
            longitude < -180 || longitude > 180)
            {
                return false;
            }
            return true;
        }

        [HttpGet("{pointId}")]
        public async Task<IResult> GetLocationAsync([FromRoute] int pointId)
        {
            //if (pointId == null || pointId <= 0)
            //{
            //    return Results.BadRequest();
            //}
            try
            {
                var result = await _storage.GetLocationAsync(pointId);
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
        public async Task<IResult> CreateLocationAsync(CreateLocationContract contract)
        {
            if (!IsPointValid(contract.Latitude, contract.Longitude))
            {
                return Results.BadRequest();
            }
            if(await _storage.IsPointExist(contract.Latitude, contract.Longitude))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _storage.CreateLocationAsync(contract);
                return Results.Created(HttpContext.Request.Path , result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("{pointId}")]
        public async Task<IResult> UpdateLocationAsync(UpdateLocationContract contract)
        {
            if (!IsPointValid(contract.Latitude, contract.Longitude) || contract.Id <= 0)
            {
                return Results.BadRequest();
            }
            if (await _storage.IsPointExist(contract.Latitude, contract.Longitude))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _storage.UpdateLocationAsync(contract);
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

        [HttpDelete("{pointId}")]
        public async Task<IResult> DeleteLocationAsync([FromRoute] int pointId)
        {
            //ТОЧКА СВЯЗАНА С ЖИВОТНЫМ
            //if (pointId == null || pointId <= 0)
            //{
            //    return Results.BadRequest();
            //}
            try
            {
                var result = await _storage.DeleteLocationAsync(pointId);
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
