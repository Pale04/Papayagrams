using DomainClasses;
using System;
using System.Collections;

namespace BussinessLogic
{
    public class GameRoomsPool
    {
        private static Hashtable _gameRooms = new Hashtable();

        /// <summary>
        /// Remove the player of the specified game room. If the game room is empty, remove the game room
        /// </summary>
        /// <param name="gameRoomCode">The game room code of the player</param>
        public static void RemovePlayerFromGameRoom(string username, string gameRoomCode)
        {
            Player player = PlayersOnlinePool.GetPlayer(username);
            GameRoom gameRoom = (GameRoom)_gameRooms[gameRoomCode];
            gameRoom.Players.Remove(player);

            if (gameRoom.Players.Count == 0)
            {
                _gameRooms.Remove(gameRoomCode);
            }
        }

        /// <summary>
        /// Get the instance of a game room based on its game room code
        /// </summary>
        /// <param name="code">The game room code to search for</param>
        /// <returns>The instance of the game room with the code specified or null if the game room with that code does not exist</returns>
        public static GameRoom GetGameRoom(string code)
        {
            return (GameRoom)_gameRooms[code];
        }

        /// <summary>
        /// Generate the room code, assign it to the game room and add it to the game rooms pool
        /// </summary>
        /// <param name="gameRoom">The game room to add</param>
        /// <returns>The new game room code assigned</returns>
        public static void AddGameRoom(GameRoom gameRoom)
        {
            string gameRoomCode = GenerateGameRoomCode();
            gameRoom.RoomCode = gameRoomCode;
            _gameRooms.Add(gameRoomCode, gameRoom);
        }

        /// <summary>
        /// Generate a random, not in use, 4 character code
        /// </summary>
        /// <returns>A random 4 character, not in use game room code</returns>
        private static string GenerateGameRoomCode()
        {
            string code;

            do
            {
                code = CodeGenerator.GenerateCode();
            } while (_gameRooms[code] != null);

            return code;
        }
    }

    internal class CodeGenerator
    {
        /// <summary>
        /// Generate a random 4 character code
        /// </summary>
        /// <returns>A string with the code</returns>
        public static string GenerateCode()
        {
            string code = string.Empty;
            Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                code += (char)random.Next(65, 90);
            }

            return code;
        }
    }
}
