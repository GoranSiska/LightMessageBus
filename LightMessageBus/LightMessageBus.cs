using System;
using System.Collections.Generic;
using System.Linq;
using LightMessageBus.Interfaces;

namespace LightMessageBus
{
    /// <summary>
    /// Light-weight message bus
    /// </summary>
    /// <remarks>
    /// Used to enable communication between objects without direct references.
    /// </remarks>
    public class LightMessageBus : IPublishers, IMessages
    {
        #region Globals

        private IList<RegistrationEntry> _register = new List<RegistrationEntry>();

        private RegistrationEntry _entry;

        #endregion

        #region Constructors

        //hide default constructor to force the use of instance property
        protected LightMessageBus() { }

        #endregion

        #region Singleton

        private static readonly Lazy<LightMessageBus> DefaultInstance = new Lazy<LightMessageBus>(()=>new LightMessageBus());
        
        public static IPublishers Default
        {
            get { return DefaultInstance.Value; }
        }

        #endregion

        #region IPublishers

        public IMessages From(object publisher)
        {
            _entry = new RegistrationEntry
            {
                PublisherHashCode = publisher.GetHashCode(),
            };

            return this;
        }

        public IMessages FromAny()
        {
            _entry = new RegistrationEntry
            {
                PublisherHashCode = -1
            };

            return this;
        }

        public void Publish(IMessage message)
        {
            foreach (var registrationEntry in MatchingSubscribers(message))
            {
                registrationEntry.Subscriber.Handle(message);   
            }
        }

        private IEnumerable<RegistrationEntry> MatchingSubscribers(IMessage message)
        {
            return _register
                .Where(re => re.PublisherHashCode == -1 || re.PublisherHashCode == message.Source.GetHashCode())
                .Where(re => re.MessageType == null || re.MessageType == message.GetType());
        }

        public bool HasRegistered(IMessageHandler subscriber)
        {
            return _register.Any(re => re.Subscriber == subscriber);
        }

        #endregion

        #region IMessages

        public IMessages Where<T>() where T : IMessage
        {
            _entry.MessageType = typeof(T);

            return this;
        }

        public void Notify(IMessageHandler subscriber)
        {
            _entry.Subscriber = subscriber;
            _register.Add(_entry);
        }

        #endregion
    }

    class RegistrationEntry
    {
        public IMessageHandler Subscriber { get; set; }
        public int PublisherHashCode { get; set; }
        public Type MessageType { get; set; }
    }
}
