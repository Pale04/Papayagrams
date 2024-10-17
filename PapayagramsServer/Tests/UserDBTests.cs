using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainClasses;
using LanguageExt;

namespace DataAccess.Tests
{
    [TestClass()]
    public class UserDBTests
    {
        //TODO: Implement set up
        //TODO: Implement tear down

        [TestMethod()]
        public void RegisterUserSuccesfulTest()
        {
            Player newPlayer = new Player()
            {
                Username = "Pale47",
                Email = "epalemolina@gmail.com",
                Password = "asdfl_´.468*-"
            };

            int expected = 1;
            int result = UserDB.RegisterUser(newPlayer);

            Assert.AreEqual(expected, result, "RegisterUserSuccesfulTest");
        }

        [TestMethod()]
        public void GetPlayerByUsernameSuccessfulTest()
        {
            //TODO: Implement set up method to insert a player in the database

            Player expected = new Player()
            {
                Id = 1,
                Username = "Pale47",
                Email = "epalemolina@gmail.com",
                Password = "asdfl_´.468*-"
            };

            Option<Player> result = UserDB.GetPlayerByUsername("Pale47");

            Assert.AreEqual(expected, result.Case, "GetPlayerByUsernameSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerByUsernameInexistentTest()
        {
            Option<Player> result = UserDB.GetPlayerByUsername("Pale");
            Assert.IsTrue(result.IsNone, "GetPlayerInexistentTest");
        }

        [TestMethod()]
        public void GetPlayerByEmailSuccessfulTest()
        {
            //TODO: Implement set up method to insert a player in the database

            Player expected = new Player()
            {
                Id = 1,
                Username = "Pale47",
                Email = "epalemolina@gmail.com",
                Password = "asdfl_´.468*-"
            };

            Option<Player> result = UserDB.GetPlayerByEmail("epalemolina@gmail.com");

            Assert.AreEqual(expected, result.Case, "GetPlayerByEmailSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerByEmailInexistentTest()
        {
            Option<Player> result = UserDB.GetPlayerByEmail("pale@gmail.com");
            Assert.IsTrue(result.IsNone, "GetPlayerByEmailInexistentTest");
        }
    }
}