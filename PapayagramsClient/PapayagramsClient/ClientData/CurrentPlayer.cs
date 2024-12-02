using PapayagramsClient.PapayagramsService;

namespace PapayagramsClient
{
    public class Configuration
    {
        public LanguageDC Language { get; set; }
        public int cursor { get; set; }
        public string pieceColor { get; set; }
    }

    public static class CurrentPlayer
    {
        public static PlayerDC Player { get; set; }

        public static Configuration Configuration { get; set; }
    }
}
