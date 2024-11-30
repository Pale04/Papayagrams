using PapayagramsClient.PapayagramsService;

namespace PapayagramsClient
{
    public class Configuration
    {
        public LanguageDC Language { get; set; }
        public int cursor { get; set; }
        public string pieceColor { get; set; }
    }

    public class CurrentPlayer
    {
        private static PlayerDC _player;

        public static PlayerDC Player { get { return _player; } set { _player = value; } }

        public static Configuration Configuration { get; set; }
    }
}
