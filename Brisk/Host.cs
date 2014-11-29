using System.Net;
using Brisk.EventPublishing;

namespace Brisk
{
    public class Host
    {
        public ICommander Commander { get; set; }
        public IEventer Eventer { get; set; }
        public Repository Repository { get; set; }
        public Publisher Publisher { get; set; }

        private IPEndPoint _endPoint;

        public Host()
        {
            Publisher = new Publisher();
            Commander = new Commander();
            Eventer = new Eventer();
        }

        public static Host Create(IPEndPoint endPoint = null)
        {
            endPoint = endPoint ?? new IPEndPoint(IPAddress.Parse("127.0.0.1"), 15150);
            return new Host(endPoint);
        }

        private Host(IPEndPoint endPoint = null) : this()
        {
            _endPoint = endPoint;
        }

        public void Init()
        {
            // discover and wire up components

            // start inital controllers
            Publisher = new Publisher();
            Publisher.Init(_endPoint);
        }
    }
}