using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AimHealth.Safari.BulkPosting.Bus.BatchRepository;
using NUnit.Framework;
using Rhino.Mocks;
using AimHealth.Safari.BulkPosting.Data;
using System.Data;

namespace AimHealth.Safari.BulkPosting.Bus.Tests
{
    [TestFixture]
    public class BulkPaymentTest
    {
        private MockRepository _mockery;
        private IUser _userMock;
        private IBatch _batchMock;

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
//            _dataAccessMock = _mockery.CreateMock<DataAccess>();
            _batchMock = _mockery.CreateMock<Batch>();
            _userMock = _mockery.CreateMock<User>(11951, "IPDuncan");
        }

        [Test]
        public void Should_return_instance_of_bulkpayment()
        {
            IBulkPayment bulkPayment = CreateBulkPayment();
            Assert.That(bulkPayment != null);
            Assert.That(bulkPayment.LedgerType == LedgerType.PayerAccountsReceivable);
            Assert.That(bulkPayment.Id == "987654321");
        }

        //[Test]
        //public void Should_add_payments_to_bulkpayment_payments()
        //{
        //    //todo - why is the mocking not working?
        //    IBulkPayment bulkPayment = CreateBulkPayment();
        //    DataTable dataTableMock = CreateDataTable();
        //    Mapper mapperMock = CreateMapper();
        //    bulkPayment.AddPayments(dataTableMock, mapperMock);
        //    Assert.IsTrue(bulkPayment.Payments.Count == 2);
        //}

        //[Test]
        //public void Should_not_add_payments_to_bulkpayment_payments()
        //{
        //    //The column name is wrong
        //    IBulkPayment bulkPayment = CreateBulkPayment();
        //    DataTable dataTableMock = CreateDataTable();
        //    Mapper mapperMock = CreateMapperWithIncorrectProperty();                                                            
        //    bulkPayment.AddPayments(dataTableMock, mapperMock);
        //    Assert.IsTrue(bulkPayment.Payments.Count == 0);
            
        //}

        [Test]
        public void Should_return_an_existing_bulkpayment()
        {
            IBulkPayment bulkPayment = CreateBulkPaymentPersisted();
            Assert.IsNotNull(bulkPayment);
            Assert.AreEqual(bulkPayment.BatchHeaderKey, 9650);
            Assert.IsNotNull(bulkPayment.Id);
        }


        [Test]
        public void Should_return_defaultcontractcode()
        {
            IBulkPayment bulkPayment = CreateBulkPayment();
            bulkPayment.DefaultInsurerContractCode = "400000001";
            Assert.AreEqual(bulkPayment.DefaultInsurerContractCode, "400000001");
        }

        [Test]
        public void Should_return_defaultcontractkey()
        {
            IBulkPayment bulkPayment = CreateBulkPayment();
            bulkPayment.DefaultInsurerContractCode = "400000001";
            Assert.That(bulkPayment.DefaultInsurerContractKey == 2690);
        }

        private IBulkPayment CreateBulkPayment()
        {
            IBulkPayment bulkPayment = new BulkPayment(LedgerType.PayerAccountsReceivable, "987654321", 10000, new DateTime(2009, 5, 1), _userMock, _batchMock);
            return bulkPayment;
        }

        private IBulkPayment CreateBulkPaymentPersisted()
        {
            IBulkPayment bulkPayment = new BulkPayment(LedgerType.PayerAccountsReceivable, 9650, _batchMock);
            return bulkPayment;
        }

        //private Mapper CreateMapper()
        //{
        //    Mapper mapperMock = new Mapper();
        //    mapperMock.Add("ClaimId", "AimClaimId");
        //    mapperMock.Add("Amount", "Amount");
        //    mapperMock.Add("InsurerContractCode", "InsurerContractCode");
        //    return mapperMock;
        //}

        //private Mapper CreateMapperWithIncorrectProperty()
        //{
        //    Mapper mapperMock = new Mapper();
        //    mapperMock.Add("ClaimId", "NotAClaimId");
        //    mapperMock.Add("Amount", "Amount");
        //    mapperMock.Add("InsurerContractCode", "InsurerContractCode");
        //    return mapperMock;
        //}

        //private DataTable CreateDataTable()
        //{
        //    DataSet dataSetMock = new DataSet("testds");
        //    DataTable dataTableMock = dataSetMock.Tables.Add("test");
        //    dataTableMock.Columns.Add("ClaimId");
        //    dataTableMock.Columns.Add("Amount");
        //    dataTableMock.Columns.Add("InsurerContractCode");
        //    //this row will be deleted
        //    dataTableMock.Rows.Add("ClaimId", "Amount", "InsurerContractCode");
        //    dataTableMock.Rows.Add("CRE01-1298-02", 30.03, "400000001");
        //    dataTableMock.Rows.Add("CRE01-1300-05", 44.56, "400000001");
        //    return dataTableMock;
        //}

        //private DataTable CreateDataTableBadData()
        //{
        //    DataSet dataSetMock = new DataSet("testds");
        //    DataTable dataTableMock = dataSetMock.Tables.Add("test");
        //    dataTableMock.Columns.Add("ClaimId");
        //    dataTableMock.Columns.Add("Amount");
        //    dataTableMock.Columns.Add("InsurerContractCode");
        //    //this row will be deleted
        //    dataTableMock.Rows.Add("ClaimId", "Amount", "InsurerContractCode");
        //    dataTableMock.Rows.Add("CRE01-1298-02", -30.03, "400000001");
        //    dataTableMock.Rows.Add("CRE01-1300-05", 44.56, "40000");
        //    dataTableMock.Rows.Add("", 44.56, "400000001");
        //    return dataTableMock;
        //}

    }
}
