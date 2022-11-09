using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.SchemeHandlers;

public interface IPaymentSchemeHandler
{
    Task<bool> ProcessPayment(Account account, decimal requestedAmount);
}