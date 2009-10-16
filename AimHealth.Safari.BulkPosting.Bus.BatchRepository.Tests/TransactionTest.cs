using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using NUnit.Framework;
using AimHealth.Safari.BulkPosting.Bus.BatchRepository;

namespace AimHealth.Safari.BulkPosting.Bus.BatchRepository.Tests
{
    [TestFixture]
    public class TransactionTest
    {
        private MockRepository _mockery;

        [SetUp]
        public void Setup()
        {
//            _mockery = new MockRepository();
        }

        [Test]
        public void Should_return_transaction_object()
        {
            ITransaction transaction = new Transaction();
            Assert.IsNotNull(transaction);
        }

        [Test]
        public void Should_return_transaction_object_with_parameters()
        {
            ITransaction transaction = new Transaction(
                123, TransactionCode.COM, 123456, 123.32, System.DateTime.Parse("6/24/2009"), System.DateTime.Parse("6/24/2009"), null, 0);
            Assert.IsNotNull(transaction);
            Assert.Equals(transaction.ClaimHeaderKey, 123456);
        }

        [Test]
        public void Should_return_transaction_object_with_payment_properties_when_passed_commission_transaction_code()
        {
            ITransaction transaction = new Transaction(
                123, TransactionCode.COM, 123456, 123.32, System.DateTime.Parse("6/24/2009"), System.DateTime.Parse("6/24/2009"), null, 0);
            Assert.IsNotNull(transaction);
            Assert.AreSame(transaction.TransactionCode.ToString(), TransactionCode.COM.ToString());
            Assert.AreSame(transaction.TransactionCategory.ToString(), TransactionCategory.PYMT.ToString());
            Assert.AreSame(transaction.TransactionType.ToString(), TransactionType.P.ToString());

        }
    }
}
