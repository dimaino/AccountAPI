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

namespace AccountAPI.Contracts
{
    public interface IEmailAccountRepository : IGenericRepository<EmailAccount>
    {
        // Default Options
        Task<IEnumerable<EmailAccount>> GetAllEmailAccountsDefaultAsync();
        Task<EmailAccount> GetEmailAccountByIdDefaultAsync(int EmailAccountId);
        Task<int> CreateEmailAccountAsync(EmailAccount EmailAccountToAdd);
        Task<int> UpdateEmailAccountAsync(EmailAccount EmailAccountToUpdate);
        Task<int> DeleteEmailAccountAsync(EmailAccount EmailAccountToDelete);
        Task<int> CountNumberOfEmailAccountsAsync();

        // Project Specific
        Task<IEnumerable<object>> GetAllEmailAccountsAsync();
        Task<object> GetEmailAccountByIdAsync(int EmailAccountId);
        bool DoesEmailAccountExist(int id);
    }
}