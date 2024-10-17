using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainClasses;

namespace DataAccess.Tests
{
    [TestClass()]
    public class UserDBTests
    {
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
        public void LogInSuccesfulTest()
        {
            Player expected = new Player()
            {
                Id = 1,
                Username = "Pale47",
                Email = "epalemolina@gmail.com",
                Password = "asdfl_´.468*-"
            };

            Player result = UserDB.LogIn("Pale47", "asdfl_´.468*-");

            Assert.AreEqual(expected, result, "LogInSuccesfulTest");
        }
    }
}