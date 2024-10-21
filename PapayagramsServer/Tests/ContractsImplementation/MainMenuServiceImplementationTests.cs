using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace Contracts.Tests
{
    [TestClass()]
    public class MainMenuServiceImplementationTests
    {
        private readonly ServiceImplementation _serviceImplementation = new ServiceImplementation();
        private readonly PlayerDC _registeredPlayer = new PlayerDC()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704"
        };

        [TestInitialize()]
        public void SetUp()
        {
            _serviceImplementation.RegisterUser(_registeredPlayer);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            using (var context = new papayagramsEntities())
            {
                context.Database.ExecuteSqlCommand("delete from [TimeAtackHistory]");
                context.Database.ExecuteSqlCommand("delete from [SuddenDeathHistory]");
                context.Database.ExecuteSqlCommand("delete from [OriginalGameHistory]");
                context.Database.ExecuteSqlCommand("delete from [UserConfiguration]");
                context.Database.ExecuteSqlCommand("delete from [UserStatus]");
                context.Database.ExecuteSqlCommand("delete from [User]");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('User', RESEED, 0)");
            }
        }

        [TestMethod()]
        public void SearchPlayerSuccessfulTest()
        {
            PlayerDC result = _serviceImplementation.SearchPlayer(_registeredPlayer.Username);
            Assert.AreEqual(_registeredPlayer, result, "SearchPlayerSuccessfulTest");
        }

        //It is the same case when the username is empty
        [TestMethod()]
        public void SearchPlayerNonExistentTest()
        {
            try
            {
                PlayerDC result = _serviceImplementation.SearchPlayer("David");
                Assert.Fail("SearchPlayerNonExistentTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(205, error.Detail.ErrorCode, "SearchPlayerNonExistentTest");
            }
        }

        [TestMethod()]
        public void SearchPlayerNullUsernameTest()
        {
            try
            {
                PlayerDC result = _serviceImplementation.SearchPlayer(null);
                Assert.Fail("SearchPlayerNullUsernameTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(205, error.Detail.ErrorCode, "SearchPlayerNullUsernameTest");
            }
        }
    }
}