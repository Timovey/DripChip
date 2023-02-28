﻿using AutoMapper;
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

        public async Task<AccountViewModel> CreateAccountAsync(CreateAccountContract contract)
        {
            var account  = _mapper.Map<Account>(contract);
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountViewModel>(contract);
        }

        public async Task<AccountViewModel> UpdateAccountAsync(UpdateAccountContract contract)
        {
            var account = await _context.Accounts.Where(el =>
                el.Id == contract.Id).FirstOrDefaultAsync();
            if(account == null )
            {
                return null;
            }
            _mapper.Map(contract, account);

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
                .Where(el => contract.FirstName != null 
                    && el.FirstName.ToLower().Contains(contract.FirstName.ToLower()))
                .Where(el => contract.LastName != null
                    && el.LastName.ToLower().Contains(contract.LastName.ToLower()))
                .Where(el => contract.Email != null
                    && el.Email.ToLower().Contains(contract.Email.ToLower()))
                .Skip(contract.From)
                .Take(contract.Size)
                .OrderBy(el => el.Id).ToListAsync();
            
            var result = new List<AccountViewModel>();
            foreach (var account in accounts)
            {
                _mapper.Map<AccountViewModel>(account);
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

        public async Task<bool> IsAccountExist(string email)
        {
            var account = await _context.Accounts.Where(el =>
                el.Email == email).FirstOrDefaultAsync();

            return account != null;
        }

        public async Task<AccountViewModel> AuthenticateAsync(LoginContract contract)
        {
            var account = await _context.Accounts.Where(el => el.Email == contract.Email
                && el.Password== contract.Password).FirstOrDefaultAsync();

            return account == null ? null : _mapper.Map<AccountViewModel>(account);
        }
    }
}
