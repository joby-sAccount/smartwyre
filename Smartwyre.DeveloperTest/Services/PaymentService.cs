using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System.Configuration;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.SchemeHandlers;

namespace Smartwyre.DeveloperTest.Services
{
 public class PaymentService : IPaymentService
    {
        private readonly IDataStoreRepository _dataStoreRepository;
        private readonly ISchemeHandlerResolver _schemeHandlerResolver;
        
        public PaymentService(IDataStoreRepository dataStoreRepository,
            ISchemeHandlerResolver schemeHandlerResolver)
        {
            _dataStoreRepository = dataStoreRepository;
            _schemeHandlerResolver = schemeHandlerResolver;
        }
        
        public async Task<MakePaymentResult> MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult();
            
            if (string.IsNullOrEmpty(request.DebtorAccountNumber))
            {
                return BuildErrorResult(result, ErrorMessages.EmptyDebtorAccount);
            }

            if (request.Amount == Decimal.Zero)
            {
                return BuildErrorResult(result, ErrorMessages.InValidAmount);
            }
            
            Account account = await _dataStoreRepository.GetAccountAsync(request.DebtorAccountNumber);

            if (account is null)
            {
                return BuildErrorResult(result, ErrorMessages.NotValidAccount);
            }

            if (account.Balance < request.Amount)
            {
                return BuildErrorResult(result, ErrorMessages.InsufficientBalance);
            }

            var schemeHandler = _schemeHandlerResolver.Resolve(request.PaymentScheme);
            
            result.Success = await schemeHandler.ProcessPayment(account, request.Amount);
            
            if (result.Success)
            {
                account.Balance -= request.Amount;
                await _dataStoreRepository.UpdateAccountAsync(account);
            }
            
            return result;
        }

        private MakePaymentResult BuildErrorResult(MakePaymentResult result, string errorMessage)
        {
            result.Success = false ;
            result.ErrorMessage = errorMessage;
            return result;
        }
    }
}
