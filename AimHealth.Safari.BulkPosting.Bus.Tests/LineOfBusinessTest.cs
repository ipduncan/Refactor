using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace AimHealth.Safari.BulkPosting.Bus.Tests
{
    [TestFixture]
    public class LineOfBusinessTest
    {
        [Test]
        public void Should_return_instance_of_LineOfBusiness_for_credit_balance()
        {
            LineOfBusiness lob = new LineOfBusiness(LineOfBusiness.Abbreviation.CB);
            Assert.IsNotNull(lob);
            Assert.AreEqual(lob.ShortName, "CB");
            Assert.AreEqual(lob.Name, "Credit Balance");
        }

        [Test]
        public void Should_return_instance_of_LineOfBusiness_for_data_mining()
        {
            LineOfBusiness lob = new LineOfBusiness(LineOfBusiness.Abbreviation.DM);
            Assert.IsNotNull(lob);
            Assert.AreEqual(lob.ShortName, "DM");
            Assert.AreEqual(lob.Name, "Data Mining");
        }

        [Test]
        public void Should_change_name_when_shortname_changes()
        {
            LineOfBusiness lob = new LineOfBusiness(LineOfBusiness.Abbreviation.CB);
            Assert.IsNotNull(lob);
            Assert.AreEqual(lob.ShortName, "CB");
            lob.ShortName = LineOfBusiness.Abbreviation.DM.ToString();
            Assert.AreEqual(lob.ShortName, "DM");
            Assert.AreEqual(lob.Name, "Data Mining");

        }


    }
}
