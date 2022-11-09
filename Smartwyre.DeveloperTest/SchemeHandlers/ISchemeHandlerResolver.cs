using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.SchemeHandlers;

public interface ISchemeHandlerResolver
{
    IPaymentSchemeHandler Resolve(PaymentScheme scheme);
}