using PapayagramsClient.Menu;
using PapayagramsClient.PapayagramsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapayagramsClient
{
    public class Configuration
    {
        public LanguageDC Language { get; set; }
        public int pieceSize { get; set; }
        public int cursor { get; set; }
    }

    public class CurrentPlayer
    {
        private static PlayerDC _player;

        public static PlayerDC Player { get { return _player; } set { _player = value; } }

        public static Configuration Configuration { get; set; }
    }
}
