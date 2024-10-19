using PapayagramsClient.PapayagramsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapayagramsClient
{
    public class CurrentPlayer
    {
        private static PlayerDC _player;

        public static PlayerDC Player { get { return _player; } set { _player = value; } }
    }
}
