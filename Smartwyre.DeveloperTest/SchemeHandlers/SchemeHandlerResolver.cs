using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.SchemeHandlers;

public class SchemeHandlerResolver : ISchemeHandlerResolver
{
    public IPaymentSchemeHandler Resolve(PaymentScheme scheme)
    {
        return scheme switch
        {
            PaymentScheme.ExpeditedPayments => new ExpeditedPaymentsSchemeHandler(),
            PaymentScheme.BankToBankTransfer => new BankToBankTransferSchemeHandler(),
            PaymentScheme.AutomatedPaymentSystem => new AutomatedPaymentSystemSchemeHandler()
        };
    }
}