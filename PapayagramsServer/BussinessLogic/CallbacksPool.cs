using System.Collections;

namespace BussinessLogic
{
    public class CallbacksPool
    {
        private static Hashtable _mainMenuPlayers = new Hashtable();
        private static Hashtable _pregamePlayers = new Hashtable();

        /// <summary>
        /// Get the player's callback channel of the main menu
        /// </summary>
        /// <param name="username">The username of the player</param>
        /// <returns>The IMainMenu callback channel of the player</returns>
        public static object GetMainMenuCallbackChannel(string username)
        {
            return _mainMenuPlayers[username];
        }

        /// <summary>
        /// Get the player's callback channel of the pregame
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>Te IPregame callback channel of the player</returns>
        public static object GetPregameCallbackChannel(string username)
        {
            return _pregamePlayers[username];
        }

        /// <summary>
        /// Add a player and its callback channel to the main menu callbacks pool
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="callbackChannel">IMainMenu Callback channel of the player</param>
        public static void PlayerArrivedToMainMenu(string username, object callbackChannel)
        {
            if (!_mainMenuPlayers.ContainsKey(username))
            {
                _mainMenuPlayers.Add(username,callbackChannel);
            }
        }

        /// <summary>
        /// Add a player and its callback channel to the pregame callbacks pool
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="callbackChannel">IPregame callback channel of the player</param>
        public static void PlayerArrivedToPregame(string username, object callbackChannel)
        {
            if (!_pregamePlayers.ContainsKey(username))
            {
                _pregamePlayers.Add(username, callbackChannel);
            }
        }

        /// <summary>
        /// Remove a player from all the callbacks pools. Use it when the player logs out
        /// </summary>
        /// <param name="username">Useranme of the player</param>
        public static void PlayerLogOutOfServer(string username)
        {
            _mainMenuPlayers.Remove(username);
            _pregamePlayers.Remove(username);
        }
    }
}
