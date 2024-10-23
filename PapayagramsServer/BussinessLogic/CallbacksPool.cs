using System.Collections;

namespace BussinessLogic
{
    public class CallbacksPool
    {
        private static Hashtable _mainMenuPlayers = new Hashtable();

        /// <summary>
        /// Get a player instance based on its username
        /// </summary>
        /// <param name="username">The username of the player</param>
        /// <returns>The instance of the player with that username</returns>
        public static object GetMainMenuCallbackChannel(string username)
        {
            return _mainMenuPlayers[username];
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
        /// Remove a player from all the callbacks pools. Use it when the player logs out
        /// </summary>
        /// <param name="username">Useranme of the player</param>
        public static void PlayerLogOutOfServer(string username)
        {
            _mainMenuPlayers.Remove(username);
        }
    }
}
