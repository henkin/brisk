using LiteDB;

namespace Brisk
{
    public interface IDbProvider 
    {
        LiteDatabase Db { get; }
    }
}