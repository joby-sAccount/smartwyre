using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface IPaymentService
    {
        Task<MakePaymentResult> MakePayment(MakePaymentRequest request);
    }
}
