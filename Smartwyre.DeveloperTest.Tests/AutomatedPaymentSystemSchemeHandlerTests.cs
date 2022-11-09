using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Smartwyre.DeveloperTest.SchemeHandlers;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class AutomatedPaymentSystemSchemeHandlerTests
{
    private AutomatedPaymentSystemSchemeHandler _handler;
    private Account _account;
        
    [SetUp]
    public void SetUp()
    {
        _handler = new AutomatedPaymentSystemSchemeHandler();
        _account = new Account();
    }
        
    [Test]
    public async Task GivenProcessPayment_WhenAllowedPaymentSchemeIsAutomatedPaymentSystemSchemeAndAccountIsActive_ThenReturnTrue()
    {
        //Arrange
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.AutomatedPaymentSystem;
        _account.Status = AccountStatus.Live;

        //Act
        var result = await _handler.ProcessPayment(_account, Decimal.One);
            
        //Assert
        Assert.IsTrue(result);
    }
        
    [Test]
    [TestCase(AllowedPaymentSchemes.ExpeditedPayments)]
    [TestCase(AllowedPaymentSchemes.BankToBankTransfer)]
    public async Task GivenProcessPayment_WhenPaymentSchemeIsNotAutomatedPaymentSystemScheme_ThenReturnFalse(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        //Arrange
        _account.AllowedPaymentSchemes = allowedPaymentSchemes;
        _account.Status = AccountStatus.Live;

        //Act
        var result = await _handler.ProcessPayment(_account, Decimal.One);
            
        //Assert
        Assert.IsFalse(result);
    }
        
    [Test]
    [TestCase(AccountStatus.Disabled)]
    [TestCase(AccountStatus.InboundPaymentsOnly)]
    public async Task GivenProcessPayment_WhenPaymentSchemeIsAutomatedPaymentSystemSchemeAndAccountNotActive_ThenReturnFalse(AccountStatus accountStatus)
    {
        //Arrange
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer;
        _account.Status = accountStatus;

        //Act
        var result = await _handler.ProcessPayment(_account, Decimal.One);
            
        //Assert
        Assert.IsFalse(result);
    }
}