using DripChip.Database.Interfaces;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.DataContracts.ViewModels;

namespace DripChip.Core.BusinessLogic
{
    public class AccountLogic
    {
        private IAccountStorage _storage;
        public AccountLogic(IAccountStorage storage)
        {
            _storage = storage;
        }

        public async Task CreateAccountAsync(CreateAccountContract contract)
        {
            if(await _storage.IsAccountExist(contract.Email))
            {
            }
            try
            {
            }
            catch(Exception ex)
            {
            }
        }

        public Task<AccountViewModel> Authenticate(LoginContract contract)
        {
            return _storage.AuthenticateAsync(contract);
        }
    }
}
