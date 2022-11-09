using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Smartwyre.DeveloperTest.SchemeHandlers;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class BankToBankTransferSchemeHandlerTests
{
    private BankToBankTransferSchemeHandler _handler;
    private Account _account;
        
    [SetUp]
    public void SetUp()
    {
        _handler = new BankToBankTransferSchemeHandler();
        _account = new Account();
    }
        
    [Test]
    public async Task GivenProcessPayment_WhenAllowedPaymentSchemeIsBankToBankTransfer_ThenReturnTrue()
    {
        //Arrange
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer;
    
        //Act
        var result = await _handler.ProcessPayment(_account, Decimal.One);
            
        //Assert
        Assert.IsTrue(result);
    }
        
    [Test]
    [TestCase(AllowedPaymentSchemes.ExpeditedPayments)]
    [TestCase(AllowedPaymentSchemes.AutomatedPaymentSystem)]
    public async Task GivenProcessPayment_WhenPaymentSchemeIsNotBankToBankTransfer_ThenReturnTrue(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        //Arrange
        _account.AllowedPaymentSchemes = allowedPaymentSchemes;
    
        //Act
        var result = await _handler.ProcessPayment(_account, Decimal.One);
            
        //Assert
        Assert.IsFalse(result);
    }
}