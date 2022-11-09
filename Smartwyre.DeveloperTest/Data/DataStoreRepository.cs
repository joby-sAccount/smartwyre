using System.Collections.Generic;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class DataStoreRepository: IDataStoreRepository
{
    private readonly Dictionary<string, Account> _accountRepository;
            
    public DataStoreRepository()
    {
        _accountRepository = new Dictionary<string, Account>();
    }

    public async Task<Account> GetAccountAsync(string accountnumber)
    {
        return await Task.FromResult(_accountRepository[accountnumber]);
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        var accoutToBeUpdated = await GetAccountAsync(account.AccountNumber);
        accoutToBeUpdated.Status = account.Status;
        accoutToBeUpdated.Balance = account.Balance;
        accoutToBeUpdated.AllowedPaymentSchemes = account.AllowedPaymentSchemes;
        _accountRepository[account.AccountNumber] = accoutToBeUpdated;
        return await Task.FromResult(accoutToBeUpdated);
    }
}