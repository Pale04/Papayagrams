using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using DomainClasses;
using Tests;

namespace Contracts.Tests
{
    [TestClass()]
    public class ApplicationSettingsServiceImplementation
    {
        private readonly ServiceImplementation _serviceImplementation = new ServiceImplementation();
        private readonly PlayerDC _registeredPlayer1 = new PlayerDC()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704"
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(new Player
            {
                Username = _registeredPlayer1.Username,
                Email = _registeredPlayer1.Email,
                Password = _registeredPlayer1.Password
            });
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
        }

        [TestMethod()]
        public void GetApplicationSettingsSuccessfulTest()
        {
            ApplicationSettingsDC expected = new ApplicationSettingsDC()
            {
                PieceColor = 1,
                SelectedLanguage = ApplicationLanguageDC.auto,
                Cursor = 1
            };
            ApplicationSettingsDC result = _serviceImplementation.GetApplicationSettings(_registeredPlayer1.Username).configuration;
            Assert.AreEqual(expected, result, "GetApplicationSettingsSuccessfulTest");
        }

        [TestMethod()]
        public void GetApplicationSettingsInvalidUsernameTest()
        {
            int expected = 205;
            int result = _serviceImplementation.GetApplicationSettings(null).returnCode;
            Assert.AreEqual(expected, result, "GetApplicationSettingsInvalidUsernameTest");
        }

        [TestMethod] 
        public void GetApplicationSettingsNonExistentUserTest()
        {
            int expected = 205;
            int result = _serviceImplementation.GetApplicationSettings("Deivid").returnCode;
            Assert.AreEqual(expected, result, "GetApplicationSettingsNonExistentUserTest");
        }

        [TestMethod()]
        public void UpdateAplicationSettingsSuccessfulTest()
        {
            int expected = 0;
            int result = _serviceImplementation.UpdateAplicationSettings(_registeredPlayer1.Username, new ApplicationSettingsDC()
            {
                PieceColor = 1,
                SelectedLanguage = ApplicationLanguageDC.english,
                Cursor = 1
            });
            Assert.AreEqual(expected, result, "UpdateAplicationSettingsSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateAplicationSettingsInvalidParametersTest()
        {
            int expected = 101;
            int result = _serviceImplementation.UpdateAplicationSettings(null, null);
            Assert.AreEqual(expected, result, "UpdateAplicationSettingsInvalidUsernameTest");
        }

        [TestMethod()]
        public void UpdateAplicationSettingsNonExistentUserTest()
        {
            int expected = 501;
            int result = _serviceImplementation.UpdateAplicationSettings("Deivid", new ApplicationSettingsDC()
            {
                PieceColor = 1,
                SelectedLanguage = ApplicationLanguageDC.english,
                Cursor = 1
            });
            Assert.AreEqual(expected, result, "UpdateAplicationSettingsNonExistentUserTest");
        }

        [TestMethod()]
        public void UpdatePasswordSuccessfulTest()
        {
            int expected = 0;
            int result = _serviceImplementation.UpdatePassword(_registeredPlayer1.Username, _registeredPlayer1.Password, "123456");
            Assert.AreEqual(expected, result, "UpdatePasswordSuccessfulTest");
        }

        [TestMethod()]
        public void UpdatePasswordInvalidParametersTest()
        {
            int expected = 101;
            int result = _serviceImplementation.UpdatePassword(null, null, null);
            Assert.AreEqual(expected, result, "UpsdatePasswordInvalidParametersTest");
        }

        [TestMethod()]
        public void UpdatePasswordNonExistentUserTest()
        {
            int expected = 503;
            int result = _serviceImplementation.UpdatePassword("Deivid", "123456", "abcdef");
            Assert.AreEqual(expected, result, "UpdatePasswordNonExistentUserTest");
        }

        [TestMethod()]
        public void UpdatePasswordInvalidCurrentPasswordTest()
        {
            int expected = 503;
            int result = _serviceImplementation.UpdatePassword(_registeredPlayer1.Username, "123456", "abcdef");
            Assert.AreEqual(expected, result, "UpdatePasswordInvalidCurrentPasswordTest");
        }

        [TestMethod()]
        public void UpdateProfileIconSuccessfulTest()
        {
            int expected = 0;
            int result = _serviceImplementation.UpdateProfileIcon(_registeredPlayer1.Username, 2);
            Assert.AreEqual(expected, result, "UpdateProfileIconSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateProfileIconSameIconTest()
        {
            int expected = 0;
            int result = _serviceImplementation.UpdateProfileIcon(_registeredPlayer1.Username, 1);
            Assert.AreEqual(expected, result, "UpdateProfileIconSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateProfileIconInvalidUsernameTest()
        {
            int expected = 101;
            int result = _serviceImplementation.UpdateProfileIcon(null, 1);
            Assert.AreEqual(expected, result, "UpdateProfileIconInvalidUsernameTest");
        }

        [TestMethod()]
        public void UpdateProfileIconInvalidProfileIconTest()
        {
            int expected = 101;
            int result = _serviceImplementation.UpdateProfileIcon(_registeredPlayer1.Username, 0);
            Assert.AreEqual(expected, result, "UpdateProfileIconInvalidProfileIconTest");
        }

        [TestMethod()]
        public void UpdateProfileIconNonExistentUserTest()
        {
            int expected = 502;
            int result = _serviceImplementation.UpdateProfileIcon("Deivid", 1);
            Assert.AreEqual(expected, result, "UpdateProfileIconNonExistentUserTest");
        }
    }
}