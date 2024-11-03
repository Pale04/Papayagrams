using DomainClasses;
using System.Collections.Generic;

namespace BussinessLogic
{
    public class PlayersPool
    {
        private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
        private static Dictionary<string, string> _accountVerificationCodes = new Dictionary<string, string>();

        public static void AddPlayer(Player player)
        {
            _players.Remove(player.Username);
            _players.Add(player.Username, player);
        }

        public static Player GetPlayer(string username)
        {
            return _players[username];
        }

        public static void RemovePlayer(string username) {
            _players.Remove(username);
        }

        /// <summary>
        /// Generate a new account verification code for the specified username and remove any previous code
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>The verification code</returns>
        public static string GenerateAccountVerificationCode(string username)
        {
            if (_accountVerificationCodes.ContainsKey(username))
            {
                _accountVerificationCodes.Remove(username);
            }
            string code;

            do
            {
                code = CodeGenerator.GenerateCode();
            } while (_accountVerificationCodes.ContainsValue(code));

            _accountVerificationCodes.Add(username,code);

            return code;
        }

        public static bool AccountVerificationCodeCorrect(string username, string code)
        {
            bool codeCorrect = false;
            if (_accountVerificationCodes.ContainsKey(username))
            {
                string codeStored = _accountVerificationCodes[username];
                codeCorrect = codeStored != null && codeStored.Equals(code);
            }
            return codeCorrect;
        }

        public static void RemoveAccountVerificationCode(string code)
        {
            _accountVerificationCodes.Remove(code);
        }
    }
}
