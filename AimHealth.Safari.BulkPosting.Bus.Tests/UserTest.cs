using System.Security.Principal;
using NUnit.Framework;
using Rhino.Mocks;

namespace AimHealth.Safari.BulkPosting.Bus.Tests
{
    [TestFixture]
    public class UserTest
    {
        private MockRepository _mockery;

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
        }

        [Test]
        public void Should_return_new_instance_of_user_when_passing_key_and_login()
        {
            const string userLogin = "IPDuncan";
            const int key = 11951;
            User user = new User(key, userLogin);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Login, userLogin);
            Assert.AreEqual(user.Key, key);

        }

        //[Test]
        //public void Should_return_current_windows_user_from_activedirectory()
        //{
        //    User user = new User();
        //    user.IdentifyWindowsUser();
        //    Assert.AreEqual(user.Login, WindowsIdentity.GetCurrent().Name);
        //    Assert.AreEqual(user.Key, 0);
        //    Assert.IsTrue(user.IsAuthenticated);
        //    //todo figure out a way to test userMock.IsAuthorizedPayerFeeBatchCreate
        //}


        
    }
}
