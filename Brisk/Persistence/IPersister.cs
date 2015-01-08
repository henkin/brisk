namespace Brisk.Persistence
{
    public interface IPersister
    {
        void Add(Entity entity);
        void Update(Entity entity);
        void Delete(Entity entity);
    }
}