using ClientTests.PapayagramsService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientTests
{
    [TestClass]
    public class LoginTests
    {
        private LoginServiceClient _host = new LoginServiceClient();
        private PlayerDC _registeredPlayer = new PlayerDC
        {
            Username = "registered",
            Password = "1230789",
            Email = "zs22013653@estudiantes.uv.mx"
        };

        [TestInitialize]
        public void SetUp()
        {
            _host.Open();

            _host.RegisterUser(_registeredPlayer);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _host.Close();
        }

        [TestMethod]
        public void RegisterCorrectUserTest()
        {
            PlayerDC player = new PlayerDC {
                Username = "Guashasha",
                Password = "123456789",
                Email = "davidcarrionr00@hotmail.com"
            };

            Assert.AreEqual(0, _host.RegisterUser(player));
        }

        [TestMethod]
        public void RegisterExistingUsernameTest()
        {
            Assert.AreEqual(201, _host.RegisterUser(_registeredPlayer));
        }

        [TestMethod]
        public void RegisterExistingEmailTest()
        {
            PlayerDC player = new PlayerDC
            {
                Username = "fulanito",
                Password = "contrasena",
                Email = "zs22013653@estudiantes.uv.mx"
            };

            Assert.AreEqual(201, _host.RegisterUser(player));
        }

        [TestMethod]
        public void RegisterNullUserTest()
        {
            Assert.AreEqual(101, _host.RegisterUser(null));
        }

        [TestMethod]
        public void RegisterEmptyUserTest()
        {
            Assert.AreEqual(101, _host.RegisterUser(new PlayerDC()));
        }

        [TestMethod]
        public void VerifyAccountIncorrectCodeTest()
        {
            Assert.AreEqual(208, _host.VerifyAccount(_registeredPlayer.Username, "1234"));
        }

        [TestMethod]
        public void VerifyAccountNonExistentUsername()
        {
            Assert.AreEqual(208, _host.VerifyAccount("abcdefg", "1234"));
        }

        [TestMethod]
        public void VerifyAccountEmptyUsername()
        {
            Assert.AreEqual(208, _host.VerifyAccount("", "1234"));
        }

        [TestMethod]
        public void VerifyAccountEmptyCode()
        {
            Assert.AreEqual(208, _host.VerifyAccount(_registeredPlayer.Username, ""));
        }

        [TestMethod]
        public void AccessAsGuest()
        {
            Assert.IsNotNull(_host.AccessAsGuest());
        }
    }
}
