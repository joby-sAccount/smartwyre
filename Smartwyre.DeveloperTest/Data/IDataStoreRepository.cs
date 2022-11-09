using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public interface IDataStoreRepository
{
    Task<Account> GetAccountAsync(string accountnumber);

    Task<Account> UpdateAccountAsync(Account account);
}