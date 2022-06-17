using MediatR;
using System;

namespace NerdStore.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregatedId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }

    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }

    }
}
