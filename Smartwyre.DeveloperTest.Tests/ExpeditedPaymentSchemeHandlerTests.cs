using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Smartwyre.DeveloperTest.SchemeHandlers;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class ExpeditedPaymentSchemeHandlerTests
{
    private ExpeditedPaymentsSchemeHandler _handler;
    private Account _account;
        
    [SetUp]
    public void SetUp()
    {
        _handler = new ExpeditedPaymentsSchemeHandler();
        _account = new Account();
    }

    [Test]
    [TestCase(1234.45, 234.45)]
    [TestCase(1234.45, 1234.45)]
    public async Task GivenProcessPayment_WhenAllowedPaymentSchemeIsExpeditedPaymentsAndAccountBalanceIsWithinLimit_ThenReturnTrue(decimal accountbalance, decimal requestedAmount)
    {
        //Arrange
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments;
        _account.Balance = accountbalance;

        //Act
        var result = await _handler.ProcessPayment(_account, requestedAmount);
        
        //Assert
        Assert.IsTrue(result);
    }

    [Test]
    [TestCase(AllowedPaymentSchemes.AutomatedPaymentSystem)]
    [TestCase(AllowedPaymentSchemes.BankToBankTransfer)]
    public async Task GivenProcessPayment_WhenPaymentSchemeIsNotExpeditedPayment_ThenReturnFalse(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        //Arrange
        _account.AllowedPaymentSchemes = allowedPaymentSchemes;
        _account.Balance = 1234.45m;

        //Act
        var result = await _handler.ProcessPayment(_account, Decimal.One);
        
        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    [TestCase(AccountStatus.Disabled)]
    [TestCase(AccountStatus.InboundPaymentsOnly)]
    public async Task GivenProcessPayment_WhenPaymentSchemeIsExpeditedPaymentsAndRequestedAmountExceedsAccountBalance_ThenReturnFalse(AccountStatus accountStatus)
    {
        //Arrange
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments;
        _account.Balance = 1234.45m;

        //Act
        var result = await _handler.ProcessPayment(_account, 1234.46m);
        
        //Assert
        Assert.IsFalse(result);
    }
}