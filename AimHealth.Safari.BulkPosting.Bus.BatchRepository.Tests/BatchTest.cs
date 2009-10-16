using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using AimHealth.Safari.BulkPosting.Data;
using AimHealth.Safari.BulkPosting.DTO;

namespace AimHealth.Safari.BulkPosting.Bus.BatchRepository.Tests
{
    [TestFixture]
    public class BatchTest
    {
        private MockRepository _mockery;
        private IDataAccess _dataAccessMock;
        private IBatch _batch;
        private BatchHeaderDTO _batchHeaderDTO;

        [SetUp]
        public void SetupResult()
        {
            _mockery = new MockRepository();
            _dataAccessMock = _mockery.CreateMock<DataAccess>();
            _batch = new Batch(_dataAccessMock);
            _batchHeaderDTO = _mockery.CreateMock<BatchHeaderDTO>();
        }

        [Test]
        public void Should_return_batch_object()
        {
            Assert.IsNotNull(_batch);
        }

        [Test]
        public void Should_identify_claims()
        {
            IList<PaymentDTO> list = CreatePayments();
            IList<PaymentDTO> returnList = _batch.IdentifyClaims(list);
            Assert.That(returnList.Count > 0);
            Assert.That(returnList[0].ClaimHeaderKey > 0);
        }

        [Test]
        public void Should_identify_contracts()
        {
            int contractKey = _batch.IdentifyContract("400000001");
            Assert.That(contractKey > 0);
        }

        [Test]
        public void Should_return_batch_header_information()
        {
            _batchHeaderDTO = _batch.GetPayerBatch(9650);
            Assert.That(_batchHeaderDTO.Key > 0);
        }

        [Test]
        public void Should_return_lists_of_transactions()
        {
            _batchHeaderDTO = _batch.GetPayerBatch(9650);
            IList<PaymentDTO> list = CreatePayments();
            _batchHeaderDTO = _batch.IdentifyTransactionTypes(list);  
            Assert.That(_batch.Transactions.Count > 0);
        }

        //[Test]
        //public void Should_create_batch()
        //{
        //    int result = _batch.CreateBatch("PayerAccountsReceivable", "UnitTest");
        //    Assert.That(result > 0);
        //}

        private IList<PaymentDTO> CreatePayments()
        {
            IList<PaymentDTO> list = new List<PaymentDTO>();
            list.Add(new PaymentDTO("DZR-399-28", null, null, "400000001", null, 10.54));
            return list;
        }

        
    }
}
