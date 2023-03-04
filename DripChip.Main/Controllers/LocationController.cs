using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Animal;
using DripChip.DataContracts.DataContracts.Location;
using DripChip.Main.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Main.Controllers
{
    [Route("locations")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private ILocationStorage _locationStorage;
        public LocationController(ILocationStorage locationStorage)
        {
            _locationStorage = locationStorage;
        }
        private bool IsPointValid(LocationBody Body)
        {
            if (Body.Latitude < -90 || Body.Latitude > 90 ||
            Body.Longitude < -180 || Body.Longitude > 180)
            {
                return false;
            }
            return true;
        }

        [HttpGet("{pointId}")]
        [NotStrict]
        public async Task<IResult> GetLocationAsync([FromRoute] int pointId)
        {
            if (pointId == null || pointId <= 0)
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _locationStorage.GetLocationAsync(pointId);
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
        public async Task<IResult> CreateLocationAsync([FromBody] LocationBody contract)
        {
            if (!IsPointValid(contract))
            {
                return Results.BadRequest();
            }
            if(await _locationStorage.IsPointExist(contract))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _locationStorage.CreateLocationAsync(contract);
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
            if (!IsPointValid(contract.Body) || contract.PointId <= 0)
            {
                return Results.BadRequest();
            }
            if (await _locationStorage.IsPointExist(contract.Body))
            {
                return Results.Conflict();
            }
            try
            {
                var result = await _locationStorage.UpdateLocationAsync(contract);
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
        public async Task<IResult> DeleteLocationAsync([FromRoute] long pointId)
        {
           
            if (pointId == null || pointId <= 0)
            {
                return Results.BadRequest();
            }
            if (await _locationStorage.GetLocationAsync(pointId) == null)
            {
                return Results.NotFound();
            }
            if (await _locationStorage.IsAnimalHasPoint(pointId))
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _locationStorage.DeleteLocationAsync(pointId);
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
