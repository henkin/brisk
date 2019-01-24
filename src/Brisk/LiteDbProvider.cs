using LiteDB;

namespace Brisk
{
    public class LiteDbProvider : IDbProvider
    {
        private static LiteDatabase _db;

        public LiteDatabase Db
        {
            get => _db;
            set => _db = value;
        }

        public LiteDbProvider()
        {
            Db = new LiteDatabase(@"Filename=Brisk.db;Mode=Exclusive;Flush=true");
        }

        ~LiteDbProvider() 
        {
            Db.Dispose();
            Db = null;
        }
    }
}