using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.Main.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DripChip.Main.Controllers
{
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountStorage _accountStorage;
        public AccountController(IAccountStorage storage)
        {
            _accountStorage = storage;
        }

        private bool IsUserAccount(ClaimsPrincipal user, int id)
        {
            var accountId = user.FindFirstValue(ClaimTypes.NameIdentifier.ToString());

            if (id.ToString() == accountId)
            {
                return true;
            }
            return false;
        }

        [HttpPost("/registration")]
        [AllowAnonymous]
        public async Task<IResult> RegistrationAsync([FromBody] AccountBody contract)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return Results.Forbid();
            }
            if (await _accountStorage.IsAccountExist(contract.Email, null))
            {
                return Results.Conflict(error: "Аккаунт с таким email уже существует");
            }
            try
            {
                return Results.Created(HttpContext.Request.Path ,await _accountStorage.CreateAccountAsync(contract));
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
                var result = await _accountStorage.GetAccountAsync(accountId);
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
                var result = await _accountStorage.GetFilteredAccountAsync(contract);

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
            if(!IsUserAccount(HttpContext.User, contract.AccountId))
            {
                return Results.Forbid();
            }
            if (await _accountStorage.IsAccountExist(contract.Body.Email, contract.AccountId))
            {
                return Results.Conflict(error: "Аккаунт с таким email уже существует");
            }
            try
            {
                var result = await _accountStorage.UpdateAccountAsync(contract);
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
            if (accountId == null || accountId <= 0)
            {
                return Results.BadRequest();
            }
            if (!IsUserAccount(HttpContext.User, accountId))
            {
                return Results.Forbid();
            }
            if(await _accountStorage.IsAnimalLinkToAccount(accountId))
            {
                return Results.BadRequest();
            }
            try
            {
                var result = await _accountStorage.DeleteAccountAsync(accountId);
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
