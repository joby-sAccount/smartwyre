using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Smartwyre.DeveloperTest.SchemeHandlers;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class SchemeHandlerResolverTests
{
    [Test]
    [TestCase(PaymentScheme.ExpeditedPayments, typeof(ExpeditedPaymentsSchemeHandler))]
    [TestCase(PaymentScheme.AutomatedPaymentSystem, typeof(AutomatedPaymentSystemSchemeHandler))]
    [TestCase(PaymentScheme.BankToBankTransfer, typeof(BankToBankTransferSchemeHandler))]
    public async Task GivenPaymentScheme_ThenReturnRelatedSchemeHandler(PaymentScheme paymentScheme, Type expectedSchemehandler)
    {
        //Arrange
        var resolver = new SchemeHandlerResolver();

        //Act
        var result = resolver.Resolve(paymentScheme);
            
        //Assert
        Assert.IsInstanceOf(expectedSchemehandler, result);
    }
}