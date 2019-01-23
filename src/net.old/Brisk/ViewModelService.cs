namespace Brisk
{
    public interface IViewModelService
    {
        T Create<T>(object parameters) where T: new();
    }

    //public class ViewModelService : Hub, IViewModelService,  IDisconnect, IHandle<Tick>
    //{
    //    public IEventer EventService { get; set; }
    //    private static List<string> _sessions = new List<string>();

    //    public ViewModelService()
    //    {
    //        new Task(() =>
    //        {
    //            while (true)
    //            {
    //                if (EventService != null)
    //                {
    //                    EventService.Raise<Tick>(new Tick() {Time = DateTime.Now});
    //                    Thread.Sleep(1000);
    //                }
    //            }
    //        }).Start();
    //    }

    //    public T Create<T>(object parameters) where T: new()
    //    {
    //        var viewModel = new T();
    //        return viewModel;
    //    }

    //    public void Meh(string time)
    //    {
            
    //    }

    //    public Task Init(string pasdjkfh)
    //    {
    //        _sessions.Add(Context.ConnectionId);
    //        var context = GlobalHost.ConnectionManager.GetHubContext<ViewModelService>();
    //        return context.Clients.Meh("asdfasdf");
            
    //    }

    //    public void Handle(Tick args)
    //    {
    //        var context = GlobalHost.ConnectionManager.GetHubContext<ViewModelService>();
    //        new Task(() => context.Clients.Meh(args.Time)).Start();
    //    }


    //    public Task Disconnect()
    //    {
    //        return Clients.leave(Context.ConnectionId, DateTime.Now.ToString());
    //    }

    //    //public Task Connect()
    //    //{
    //    //    return new Task(() => ;
    //    //    //return Clients.joined(Context.ConnectionId, DateTime.Now.ToString());
    //    //}

    //    public Task Reconnect(IEnumerable<string> groups)
    //    {
    //        return Clients.rejoined(Context.ConnectionId, DateTime.Now.ToString());
    //    }
    //}
}