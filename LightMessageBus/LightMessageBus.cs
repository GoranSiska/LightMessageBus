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

        private int _publisherHashCode;

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
            _publisherHashCode = publisher.GetHashCode();

            return this;
        }

        public IMessages FromAny()
        {
            _publisherHashCode = -1;

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
                .Where(re=>re.PublisherHashCode == -1 || re.PublisherHashCode == message.Source.GetHashCode());
        }

        public bool HasRegistered(IMessageHandler subscriber)
        {
            return _register.Any(re => re.Subscriber == subscriber);
        }

        #endregion

        #region IMessages

        public void Notify(IMessageHandler subscriber)
        {
            _register.Add(new RegistrationEntry
            {
                Subscriber = subscriber, 
                PublisherHashCode = _publisherHashCode
            });
        }

        #endregion
    }

    class RegistrationEntry
    {
        public IMessageHandler Subscriber { get; set; }
        public int PublisherHashCode { get; set; }
    }
}
