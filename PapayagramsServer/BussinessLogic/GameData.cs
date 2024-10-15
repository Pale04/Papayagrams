using DomainClasses;
using System;
using System.Collections;
using System.ServiceModel;

namespace BussinessLogic
{
    public class GameData
    {
        private static Hashtable GameRooms = new Hashtable();

        /// <summary>
        /// Remove the player of the specified operation context from the game room of the specified code
        /// </summary>
        /// <param name="context">The context of the player to remove</param>
        /// <param name="gameRoomCode">The game room code of the player</param>
        public static void RemovePlayerFromGameRoom(string username, string gameRoomCode)
        {
            Player player = PlayerData.GetPlayerByUsername(username);
            GameRoom gameRoom = (GameRoom)GameRooms[gameRoomCode];
            gameRoom.Players.Remove(player);
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
        /// Add game room to the hashtable of game rooms and generate its game room code
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
