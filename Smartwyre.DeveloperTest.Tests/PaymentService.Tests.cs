using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.SchemeHandlers;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private Mock<IDataStoreRepository> _mockDataStoreRepository;
        private Mock<ISchemeHandlerResolver> _mockSchemeResolver;
        private PaymentService _paymentService;
        
        [SetUp]
        public void Setup()
        {
            //Arrange
            _mockDataStoreRepository = new Mock<IDataStoreRepository>();
            _mockSchemeResolver = new Mock<ISchemeHandlerResolver>();

            _paymentService = new PaymentService(_mockDataStoreRepository.Object,
                _mockSchemeResolver.Object);
        }
        
        [Test]
        public async Task GivenPaymentRequest_WhenEmptyDebtorAccountNumber_ThenReturnUnSuccessfulMakePaymentResult()
        {
            //Arrange
            var makepaymentRequest = new MakePaymentRequest();

            //Act
            var result = await _paymentService.MakePayment(makepaymentRequest);
            
            //Assert
            Assert.IsInstanceOf<MakePaymentResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(ErrorMessages.EmptyDebtorAccount, result.ErrorMessage);
        }
        
        [Test]
        public async Task GivenPaymentRequest_WhenInValidAmount_ThenReturnUnSuccessfulMakePaymentResult()
        {
            //Arrange
            var makepaymentRequest = new MakePaymentRequest();
            makepaymentRequest.DebtorAccountNumber = "debtorAccountNumber";
            makepaymentRequest.Amount = 0;

            //Act
            var result = await _paymentService.MakePayment(makepaymentRequest);
            
            //Assert
            Assert.IsInstanceOf<MakePaymentResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(ErrorMessages.InValidAmount, result.ErrorMessage);
        }
        
        [Test]
        public async Task GivenPaymentRequest_WhenDebtorAccountDoNotExistInAccountStore_ThenReturnUnSuccessfulMakePaymentResult()
        {
            //Arrange
            var makepaymentRequest = new MakePaymentRequest();
            makepaymentRequest.Amount = 1;
            makepaymentRequest.DebtorAccountNumber = "debtorAccountNumber";
            
            //Act
            var result = await _paymentService.MakePayment(makepaymentRequest);
            
            //Assert
            Assert.IsInstanceOf<MakePaymentResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(ErrorMessages.NotValidAccount, result.ErrorMessage);
        }
        
        [Test]
        public async Task GivenPaymentRequest_WhenDebtorAccountDoNotHaveSufficientBalance_ThenReturnUnSuccessfulMakePaymentResult()
        {
            //Arrange
            var makepaymentRequest = new MakePaymentRequest();
            makepaymentRequest.Amount = 1;
            makepaymentRequest.DebtorAccountNumber = "debtorAccountNumber";
            var debtorAccount = new Account();
            debtorAccount.Balance = 0;
            _mockDataStoreRepository.Setup(x =>
                x.GetAccountAsync(It.IsAny<string>())).ReturnsAsync(debtorAccount);
            
            //Act
            var result = await _paymentService.MakePayment(makepaymentRequest);
            
            //Assert
            Assert.IsInstanceOf<MakePaymentResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(ErrorMessages.InsufficientBalance, result.ErrorMessage);
        }
        
        [Test]
        public async Task GivenPaymentRequest_WhenProcessPaymentHasFailed_ThenReturnUnSuccessfulMakePaymentResult()
        {
            //Arrange
            Mock<IPaymentSchemeHandler> _mockPaymentHandler = new Mock<IPaymentSchemeHandler>();
            var makepaymentRequest = new MakePaymentRequest();
            makepaymentRequest.Amount = 1;
            _mockDataStoreRepository.Setup(x =>
                x.GetAccountAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<Account>()));
            _mockPaymentHandler.Setup(x =>
                x.ProcessPayment(It.IsAny<Account>(), It.IsAny<decimal>())).ReturnsAsync(false);
            _mockSchemeResolver.Setup(x => x.Resolve(It.IsAny<PaymentScheme>()))
                .Returns(_mockPaymentHandler.Object);
            
            
            //Act
            var result = await _paymentService.MakePayment(makepaymentRequest);
            
            //Assert
            Assert.IsInstanceOf<MakePaymentResult>(result);
            Assert.IsFalse(result.Success);
            _mockDataStoreRepository.Verify(x => 
                x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
        }
        
        [Test]
        public async Task GivenPaymentRequest_WhenProcessPaymentHasSucceded_ThenReturnUnSuccessfulMakePaymentResult()
        {
            //Arrange
            Mock<IPaymentSchemeHandler> _mockPaymentHandler = new Mock<IPaymentSchemeHandler>();
            var makepaymentRequest = new MakePaymentRequest();
            makepaymentRequest.Amount = 1;
            makepaymentRequest.DebtorAccountNumber = "debtorAccountNumber";
            var debtorAccount = new Account();
            debtorAccount.Balance = 1;
            _mockDataStoreRepository.Setup(x =>
                x.GetAccountAsync(It.IsAny<string>())).ReturnsAsync(debtorAccount);
            _mockPaymentHandler.Setup(x =>
                x.ProcessPayment(It.IsAny<Account>(), It.IsAny<decimal>())).ReturnsAsync(true);
            _mockSchemeResolver.Setup(x => x.Resolve(It.IsAny<PaymentScheme>()))
                .Returns(_mockPaymentHandler.Object);
            
            
            //Act
            var result = await _paymentService.MakePayment(makepaymentRequest);
            
            //Assert
            Assert.IsInstanceOf<MakePaymentResult>(result);
            Assert.IsTrue(result.Success);
            _mockDataStoreRepository.Verify(x => 
                x.UpdateAccountAsync(It.IsAny<Account>()), Times.Once);
        }
    }
}
