using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace Contracts.Tests
{
    [TestClass()]
    public class MainMenuServiceImplementationTests
    {
        private readonly ServiceImplementation _serviceImplementation = new ServiceImplementation();
        private readonly PlayerDC _registeredPlayer1 = new PlayerDC()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704"
        };
        private readonly PlayerDC _registeredPlayer2 = new PlayerDC()
        {
            Id = 2,
            Username = "David04",
            Email = "david@gmail.com",
            Password = "040704"
        };

        [TestInitialize()]
        public void SetUp()
        {
            _serviceImplementation.RegisterUser(_registeredPlayer1);
            _serviceImplementation.RegisterUser(_registeredPlayer2);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
        }

        [TestMethod()]
        public void SearchPlayerSuccessfulTest()
        {
            //todo
        }

        //It is the same case when the username is empty
        [TestMethod()]
        public void SearchPlayerNonExistentTest()
        {
            //todo
        }

        [TestMethod()]
        public void SearchPlayerNullUsernameTest()
        {
            //todo
        }

        [TestMethod]
        public void SendFriendRequestSuccessfulTest()
        {
            
        }
    }
}