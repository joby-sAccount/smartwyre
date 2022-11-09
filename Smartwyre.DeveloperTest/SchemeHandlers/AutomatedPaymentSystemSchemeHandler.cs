using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.SchemeHandlers;

public class AutomatedPaymentSystemSchemeHandler : IPaymentSchemeHandler
{
    public async Task<bool> ProcessPayment(Account account, decimal requestedAmount)
    {
        return await Task.FromResult(account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem)
                                     && account.Status == AccountStatus.Live);
    }
}