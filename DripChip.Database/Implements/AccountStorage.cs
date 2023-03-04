using AutoMapper;
using DripChip.Database.Extensions;
using DripChip.Database.Interfaces;
using DripChip.Database.Models;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.DataContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Database.Implements
{
    public class AccountStorage : IAccountStorage
    {
        private readonly DripChipContext _context;
        private readonly IMapper _mapper;
        public AccountStorage(
            DripChipContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountViewModel> CreateAccountAsync(AccountBody contract)
        {

            var account  = _mapper.Map<Account>(contract);
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<AccountViewModel> UpdateAccountAsync(UpdateAccountContract contract)
        {
            var account = await _context.Accounts.Where(el =>
                el.Id == contract.AccountId).FirstOrDefaultAsync();
            if(account == null )
            {
                return null;
            }
            _mapper.Map(contract.Body, account);

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<AccountViewModel> GetAccountAsync(int accountId)
        {
            var account =  await _context.Accounts.Where(el => 
                el.Id == accountId).FirstOrDefaultAsync();
            return account == null ? null : _mapper.Map<AccountViewModel>(account);
        }

        public async Task<IList<AccountViewModel>> GetFilteredAccountAsync(GetFilteredAccountContract contract)
        {
            var accounts = await _context.Accounts
                .WhereIf(contract.FirstName != null, el => el.FirstName.ToLower().Contains(contract.FirstName.ToLower()))
                .WhereIf(contract.LastName != null, el => el.LastName.ToLower().Contains(contract.LastName.ToLower()))
                .WhereIf(contract.Email != null, el => el.Email.ToLower().Contains(contract.Email.ToLower()))
                .OrderBy(el => el.Id)
                .Skip(contract.From)
                .Take(contract.Size)
                .ToListAsync();
            
            var result = new List<AccountViewModel>();
            foreach (var account in accounts)
            {
                result.Add(_mapper.Map<AccountViewModel>(account));
            }
            return result;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _context.Accounts.Where(el =>
                el.Id == accountId).FirstOrDefaultAsync();
            if(account == null)
            {
                return false;
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsAccountExist(string email, int? id)
        {
            //получаем аккаунт по почте
            var account = await _context.Accounts.Where(el =>
                el.Email == email).FirstOrDefaultAsync();

            //если проверка еще по Id
            if(id != null && account != null)
            {
                return account.Id != id;
            }
            return account != null;
        }

        public async Task<AccountViewModel> AuthenticateAsync(LoginContract contract)
        {
            var account = await _context.Accounts.Where(el => el.Email == contract.Email
                && el.Password== contract.Password).FirstOrDefaultAsync();

            return account == null ? null : _mapper.Map<AccountViewModel>(account);
        }

        public async Task<bool> IsAnimalLinkToAccount(int accountId)
        {
            var animals = await _context.Animals.Where(el => el.ChipperId== accountId)
                .ToListAsync();

            if(animals != null && animals.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
