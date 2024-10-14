using DomainClasses;
using System;
using System.Collections;
using System.ServiceModel;

namespace BussinessLogic
{
    public class ServerData
    {
        private static Hashtable Players = new Hashtable();
        private static Hashtable GameRooms = new Hashtable();

        /// <summary>
        /// Get a player instance based on its operation context
        /// </summary>
        /// <param name="context">The operation context of the user</param>
        /// <returns>The instance of the player with that operation context</returns>
        public static Player GetPlayerByContext(OperationContext context)
        {
            return Players[context] as Player;
        }

        /// <summary>
        /// Get the operation context of a player based on its instance
        /// </summary>
        /// <param name="player">The instance of the player</param>
        /// <returns>The operation context of the specified player</returns>
        public static OperationContext GetPlayerContext(Player player)
        {
            foreach (OperationContext context in Players.Keys)
            {
                if (Players[context] == player)
                {
                    return context;
                }
            }

            return null;
        }

        /// <summary>
        /// Add a player and its operation context to the list of connected players
        /// </summary>
        /// <param name="player">The instance of the player</param>
        /// <param name="playerContext">The context of the player to add</param>
        public static void AddPlayer(Player player, OperationContext playerContext)
        {
            Players.Add(playerContext, player);
        }

        /// <summary>
        /// Get an instance of a game room based on its game room code
        /// </summary>
        /// <param name="code">The game room code to search for</param>
        /// <returns>The instance of the game room with the code specified</returns>
        public static GameRoom GetGameRoom(string code)
        {
            return (GameRoom)GameRooms[code];
        }

        /// <summary>
        /// Add game room to map of all game rooms and generate its game room code
        /// </summary>
        /// <param name="gameRoom">The game room to add</param>
        /// <returns>The new game room code assigned</returns>
        public static string AddGameRoom(GameRoom gameRoom)
        {
            string gameRoomCode = GenerateGameRoomCode();
            gameRoom.RoomCode = gameRoomCode;
            GameRooms.Add(gameRoomCode, gameRoom);
            return gameRoomCode;
        }

        /// <summary>
        /// Generate a random, not in use, 4 character code
        /// </summary>
        /// <returns>A random 4 character, not in use game room code</returns>
        private static string GenerateGameRoomCode()
        {
            string code = string.Empty;
            Random random = new Random();

            do
            {
                for (int i = 0; i < 4; i++)
                {
                    code += (char)random.Next(65, 91);
                }
            } while (GameRooms[code] != null);

            return code;
        }
    }
}
