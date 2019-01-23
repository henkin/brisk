using System.Threading.Tasks;
using Brisk.Events;
using Brisk.Persistence;

namespace Brisk
{
    public interface IService
    {
    }

    public class Service : IService
    {
        public IEventService EventService { get; set; }
        public IRepository Repository { get; set; }

        protected Task Delete(Entity delete)
        {
            return EventService.Raise(new PersistenceEvent(delete, PersistenceAction.Delete));
        }

        protected Task Add(Entity add)
        {
            return EventService.Raise(new PersistenceEvent(add, PersistenceAction.Create));
        }

		protected Task Update(Entity add)
		{
			return EventService.Raise(new PersistenceEvent(add, PersistenceAction.Update));
		}
    }
}