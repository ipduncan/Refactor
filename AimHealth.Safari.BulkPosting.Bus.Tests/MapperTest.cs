using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace AimHealth.Safari.BulkPosting.Bus.Tests
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void GetPaymentPropertiesTest()
        {
            Mapper mapper = new Mapper();
            StringCollection propertyList = mapper.GetPaymentProperties();            
            Assert.That(propertyList.Count == 12);
            Assert.IsFalse(propertyList.Contains("Fee"));
            Assert.IsTrue(propertyList.Contains("Amount"));

        }

        [Test]
        public void CreateMapTest()
        {
            Mapper mapper = new Mapper();
            StringCollection propertyList = mapper.GetPaymentProperties();
            mapper.Add("ClaimID", propertyList[0]);
            mapper.Add("Fee", propertyList[1]);
            mapper.Add("UID", propertyList[2]);
            mapper.Add("InsurerContractCode", propertyList[11]);

            //StringDictionary dict = mapper.GetCurrentMap();
            Assert.That(mapper.Count == 4);
            Assert.IsTrue(mapper.ContainsKey("Fee"));
            Assert.IsTrue(mapper["Fee"] == "Amount");


        }

    }
}
