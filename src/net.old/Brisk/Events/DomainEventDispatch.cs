using System;

namespace Brisk.Events
{
    public class DomainEventDispatch
    {
        public Guid ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string HandlerName { get; set; }
        public DateTime CompletedAt { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public bool IsSuccessful { get; set; }

        public DomainEventDispatch()
        {
            ID = Guid.NewGuid();
        }

        public DomainEventDispatch(string handlerName)
            : this()
        {
            CreatedAt = DateTime.UtcNow;
            this.HandlerName = handlerName;
        }

        public void Complete()
        {
            CompletedAt = DateTime.UtcNow;
            this.IsSuccessful = true;
        }
        public void Error(Exception e)
        {
            CompletedAt = DateTime.UtcNow;
            this.ExceptionMessage = e.Message;
            this.ExceptionStackTrace = e.StackTrace;
            this.IsSuccessful = false;
        }

        public override string ToString()
        {
            string message;
            if (IsSuccessful)
                message = (CompletedAt - CreatedAt).Milliseconds + " ms";
            else
                message = string.Format("{0}, {1}", ExceptionMessage, ExceptionStackTrace);

            return string.Format("{0}: {1}", HandlerName, message);
        }

    }
}
