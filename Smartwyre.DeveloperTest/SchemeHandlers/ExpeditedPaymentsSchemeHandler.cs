using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.SchemeHandlers;

public class ExpeditedPaymentsSchemeHandler : IPaymentSchemeHandler
{
    public async Task<bool> ProcessPayment(Account account, decimal requestedAmount)
    {
        return await Task.FromResult(account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.ExpeditedPayments)
                                     && account.Balance >= requestedAmount);

    }
}