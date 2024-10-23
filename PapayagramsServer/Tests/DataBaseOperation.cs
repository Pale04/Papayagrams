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
    }
}
