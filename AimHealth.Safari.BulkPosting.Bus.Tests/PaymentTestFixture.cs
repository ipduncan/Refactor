using System;
using AimHealth.Safari.BulkPosting.Exceptions;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Specialized;

namespace AimHealth.Safari.BulkPosting.Bus.Tests
{
    [TestFixture]
    public class WhenPaymentIsCreated
    {
        [Test]
        public void Should_return_payment_object()
        {
            Payment payment = new Payment("AAA23-3445-2", 23.45, "123456789", "123456", "400000001", "40000");
            Assert.IsNotNull(payment);
        }

        [Test]
        public void Should_populate_inscode_when_null_and_contractcode_is_provided()
        {
            Payment payment = new Payment("AAA23-3445-2", 23.45, "123456789", "123456", "400000001", null);
            Assert.AreEqual("40000", payment.InsurerCode);
        }

        [Test] 
        [ExpectedException(typeof(InvalidDataInsurerInformationException))]
        public void Should_fail_when_contractcode_and_insurercode_are_invalid()
        {
            Payment payment = new Payment("AAA23-3445-2", 23.45, "123456789", "123456", "400000001", "50000");
            payment = new Payment("AAA23-3445-2", 23.45, "123456789", "123456", "400001", "40000");
            payment = new Payment("AAA23-3445-2", 23.45, "123456789", "123456", "400000001", "400000001");
        }

        [Test]
        public void ShouldRoundAmountNearestCent()
        {
            for (double paymentAmount = -10; paymentAmount < 10; paymentAmount += .001 )
            {
                Payment payment = CreateSUT(paymentAmount);
                Assert.AreEqual(payment.Amount, Math.Round(paymentAmount, 2));
            }
                
        }

        private static Payment CreateSUT(double amount)
        {
            return new Payment("CRE01-1300-05", amount, null, null, "400000001", null);
        }
    }

    [TestFixture]
    public class WhenUsingMethods
    {
        
        [Test]
        public void Should_return_list_of_properties()
        {
            Payment payment = new Payment();
            StringCollection properties = payment.GetListOfProperties();
            Assert.IsNotEmpty(properties);
        }

        [Test]
        public void Should_set_properties()
        {
            Payment payment = new Payment();
            payment.SetProperty("aimclaimid", "RED-345-12");
            Assert.AreEqual(payment.AimClaimId , "RED-345-12");
        }

        [Test]
        public void Should_return_claimid_from_Vendorreferenceid()
        {
            Payment payment = new Payment();
            payment.SetProperty("VendorReferenceId", "6081934_10CXT0904032");
            Assert.AreEqual(payment.AimClaimId, "10CXT0904032");        
        }

        [Test]
        public void Should_return_claimheaderkey_from_Vendorreferenceid()
        {
            Payment payment = new Payment();
            payment.SetProperty("VendorReferenceId", "6081934_10CXT0904032");
            Assert.AreEqual(payment.ClaimHeaderKey , Int32.Parse("6081934"));                    
        }

        private static Payment CreateSUT()
        {
            return new Payment("CRE01-1300-05", 45.54, "1234567890", "987654", "400000001", "40000");
        }

    }

}