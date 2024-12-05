using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapayagramsClient;

namespace ClientTests
{
    [TestClass]
    public class WordCheckerTests
    {
        [TestMethod]
        public void CheckCorrectSpanishWord()
        {
            Assert.IsTrue(WordChecker.ValidWord("hola", PapayagramsClient.PapayagramsService.LanguageDC.Spanish));
        }

        [TestMethod]
        public void CheckCorrectAccentSpanishWord()
        {
            Assert.IsTrue(WordChecker.ValidWord("haras", PapayagramsClient.PapayagramsService.LanguageDC.Spanish));
        }

        [TestMethod]
        public void CheckIncorrectSpanishWord()
        {
            Assert.IsFalse(WordChecker.ValidWord("rtnrrrrh", PapayagramsClient.PapayagramsService.LanguageDC.Spanish));
        }

        [TestMethod]
        public void CheckCorrectEnglishWord()
        {
            Assert.IsTrue(WordChecker.ValidWord("mine", PapayagramsClient.PapayagramsService.LanguageDC.English));
        }

        [TestMethod]
        public void CheckIncorrectEnglishWord()
        {
            Assert.IsFalse(WordChecker.ValidWord("rtnrrrrh", PapayagramsClient.PapayagramsService.LanguageDC.Spanish));
        }

        [TestMethod]
        public void CheckOneLetter()
        {
            Assert.IsFalse(WordChecker.ValidWord("l", PapayagramsClient.PapayagramsService.LanguageDC.Spanish));
        }
    }
}
