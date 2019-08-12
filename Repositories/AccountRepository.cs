using AccountAPI.Models;
using AccountAPI.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.DTOs;

namespace AccountAPI.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly Context _Context;
        public AccountRepository(Context Context) : base(Context)
        {
            _Context = Context;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsDefaultAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<Account> GetAccountByIdDefaultAsync(int AccountId)
        {
            return await FindByCondition(a => a.AccountId == AccountId).FirstOrDefaultAsync();
        }

        public async Task<string> CreateAccountAsync(Account AccountToAdd)
        {
            if(!FindAnyByCondition(a => (a.EmailAccountId == AccountToAdd.EmailAccountId && a.PlatformId == AccountToAdd.PlatformId)))
            {
                AccountToAdd.AccountId = GetNextAccountId() + 1;
                Create(AccountToAdd);
                await SaveAsync();
                return $"Account with the id: {AccountToAdd.AccountId} has been added.";
            }
            return $"Account was unable to be created because account already exists.";
        }

        public async Task<string> UpdateAccountAsync(Account AccountToUpdate)
        {
            if(!FindAnyByCondition(a => a.AccountId == AccountToUpdate.AccountId))
            {
                Update(AccountToUpdate);
                await SaveAsync();
                return $"Account with the id: {AccountToUpdate.AccountId} has been updated.";
            }
            return $"No account exists with the id: {AccountToUpdate.AccountId}.";
        }

        public async Task<string> DeleteAccountAsync(Account AccountToDelete)
        {

            string codeIdsToDelete = "";
            var CodeListToDelete = _Context.Codes.Where(c => (c.EmailAccountId == AccountToDelete.EmailAccountId && c.PlatformId == AccountToDelete.PlatformId));
            foreach(var code in CodeListToDelete)
            {
                codeIdsToDelete += $" {code.CodeId}";
            }
            _Context.Codes.RemoveRange(CodeListToDelete);
            Delete(AccountToDelete);
            await SaveAsync();
            return codeIdsToDelete;
        }

        public async Task<int> CountNumberOfAccountsAsync()
        {
            return await CountAsync();
        }

        public async Task<IEnumerable<object>> GetAllAccountsAsync()
        {
            return await GetAll().Include(a => a.EmailAccount).Include(a => a.Platform).Include(a => a.Event).Include(a => a.Codes).Select(a => new 
            {
                Id = a.AccountId,
                Username = a.Username,
                Password = a.Password,
                Email = a.EmailAccount.Email,
                EmailPassword = a.EmailAccount.EmailPassword,
                Platform = a.Platform.Name,
                Event = a.Event.Name
            }).ToListAsync();
        }

        public async Task<object> GetAccountByIdAsync(int AccountId)
        {
            return await FindByCondition(a => a.AccountId == AccountId).Include(a => a.EmailAccount).Include(a => a.Platform).Include(a => a.Event).Include(a => a.Codes).Select(a => new 
            {
                Id = a.AccountId,
                Username = a.Username,
                Password = a.Password,
                Email = a.EmailAccount.Email,
                EmailPassword = a.EmailAccount.EmailPassword,
                Platform = a.Platform.Name,
                Event = a.Event.Name,
                Codes = a.Codes.Select(c => new {
                    Code = c.CodeString
                })
            }).FirstOrDefaultAsync();
        }

        public int GetNextAccountId()
        {
            return _Context.Accounts.Max(a => a.AccountId);
        }

        // public async Task<Account> FindAnyAccountId(Account FindAccount)
        // {
        //     return await FindAnyByCondition();
        // }

        public async Task<IEnumerable<Account>> GetAllAccountsByPlatformId(int id)
        {
            var Accounts = await FindByCondition(a => a.PlatformId == id).ToListAsync();
            return Accounts;
        }
    }
}