using DripChip.DataContracts.DataContracts.Auth;
using DripChip.DataContracts.ViewModels;

namespace DripChip.Database.Interfaces
{
    public interface IAccountStorage
    {
        public Task<AccountViewModel> CreateAccountAsync(CreateAccountContract contract);

        public Task<AccountViewModel> UpdateAccountAsync(UpdateAccountContract contract);

        public Task<AccountViewModel> GetAccountAsync(int accountId);

        public Task<IList<AccountViewModel>> GetFilteredAccountAsync(GetFilteredAccountContract contract);

        public Task<bool> DeleteAccountAsync(int accountId);

        public Task<bool> IsAccountExist(string email);

        public Task<AccountViewModel> AuthenticateAsync(LoginContract contract);

    }
}
