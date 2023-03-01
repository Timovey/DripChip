using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.Main.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Main.Controllers
{
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountStorage _storage;
        public AccountController(IAccountStorage storage)
        {
            _storage = storage;
        }

        [HttpPost("/registration")]
        [AllowAnonymous]
        public async Task<IResult> RegistrationAsync([FromBody] CreateAccountContract contract)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return Results.Forbid();
            }
            if (await _storage.IsAccountExist(contract.Email))
            {
                return Results.Conflict(error: "Аккаунт с таким email уже существует");
            }
            try
            {
                return Results.Created(HttpContext.Request.Path ,await _storage.CreateAccountAsync(contract));
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpGet("/accounts/{accountId}")]
        [NotStrict]
        public async Task<IResult> GetAccountAsync([FromRoute] int accountId)
        {
            if (accountId == null || accountId <= 0)
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _storage.GetAccountAsync(accountId);
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

        [HttpGet("/accounts/search")]
        [NotStrict]
        public async Task<IResult> GetFilteredAccountAsync([FromQuery] GetFilteredAccountContract contract)
        {
            try
            {
                var result = await _storage.GetFilteredAccountAsync(contract);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("/accounts/{accountId}")]
        public async Task<IResult> UpdateAccountAsync(UpdateAccountContract contract)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!
            //проверка на пробелы и обновление не своего аккаунта
            if (await _storage.IsAccountExist(contract.Email))
            {
                return Results.Conflict(error: "Аккаунт с таким email уже существует");
            }
            try
            {
                var result = await _storage.UpdateAccountAsync(contract);
                if (result == null)
                {
                    return Results.Forbid();
                }
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpDelete("/accounts/{accountId}")]
        public async Task<IResult> DeleteAccountAsync([FromRoute] int accountId)
        {
            //ПРОВЕРКА НА ЖИВОТНОЕ
            //И не свой аккаунт
            if (accountId == null || accountId <= 0)
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _storage.DeleteAccountAsync(accountId);
                if (result == null || !result)
                {
                    return Results.Forbid();
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
