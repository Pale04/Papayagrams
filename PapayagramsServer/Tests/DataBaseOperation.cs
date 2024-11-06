namespace Tests
{
    internal class DataBaseOperation
    {
        public static void RebootDataBase()
        {
            using (var context = new papayagramsEntities())
            {
                context.Database.ExecuteSqlCommand("delete from [UserRelationship]");
                context.Database.ExecuteSqlCommand("delete from [TimeAtackHistory]");
                context.Database.ExecuteSqlCommand("delete from [SuddenDeathHistory]");
                context.Database.ExecuteSqlCommand("delete from [OriginalGameHistory]");
                context.Database.ExecuteSqlCommand("delete from [UserConfiguration]");
                context.Database.ExecuteSqlCommand("delete from [UserStatus]");
                context.Database.ExecuteSqlCommand("delete from [User]");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('User', RESEED, 0)");
            }
        }

        public static void CreateGameHistoryPlayer(int userId)
        {
            using (var context = new papayagramsEntities())
            {
                context.Database.ExecuteSqlCommand($"update [OriginalGameHistory] SET wonGames = 10, lostGames = 50 where userId = {userId}");
                context.Database.ExecuteSqlCommand($"update [TimeAtackHistory] SET wonGames = 5, lostGames = 25 where userId = {userId}");
                context.Database.ExecuteSqlCommand($"update [SuddenDeathHistory] SET wonGames = 3, lostGames = 100 where userId = {userId}");
            }
        }
    }
}
