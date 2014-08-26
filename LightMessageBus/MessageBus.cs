using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using LightMessageBus.Interfaces;

namespace LightMessageBus
{
    /// <summary>
    /// Light-weight message bus
    /// </summary>
    /// <remarks>
    /// Used to enable communication between objects without direct references.
    /// </remarks>
    public class MessageBus : IPublishers, IMessages
    {
        #region Globals

        private readonly IList<RegistrationEntry> _register = new List<RegistrationEntry>();

        private RegistrationEntry _entry;

        #endregion

        #region Constructors

        //hide default constructor to force the use of instance property
        private MessageBus() { }

        #endregion

        #region Singleton

        private static readonly Lazy<MessageBus> DefaultInstance = new Lazy<MessageBus>(()=>new MessageBus());
        
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

        public void Publish<T>(T message) where T : IMessage
        {
            foreach (var subscriber in MatchingSubscribers<T>(message))
            {
                subscriber.Handle(message);   
            }
        }

        private IEnumerable<IMessageHandler<T>> MatchingSubscribers<T>(IMessage message) where T : IMessage
        {
            return _register
                .Where(re => re.PublisherHashCode == -1 || re.PublisherHashCode == message.Source.GetHashCode())
                .Where(re => re.MessageType == null || re.MessageType == message.GetType() || (re.OrDerived && message.GetType().GetTypeInfo().IsSubclassOf(re.MessageType)))
                .Select(re => re.Subscriber.Target)
                .OfType<IMessageHandler<T>>();
        }

        public bool HasRegistered<T>(IMessageHandler<T> subscriber) where T : IMessage
        {
            return _register.Any(re => re.Subscriber.Target == subscriber);
        }

        #endregion

        #region IMessages

        public IMessages<T> Where<T>() where T : IMessage
        {
            _entry.MessageType = typeof(T);

            return GenericLightMessageBus<T>.Default;
        }

        public void Notify<T>(IMessageHandler<T> subscriber) where T : IMessage
        {
            _entry.Subscriber = new WeakReference(subscriber);
            _register.Add(_entry);
        }

        internal void OrDerived()
        {
            _entry.OrDerived = true;
        }

        #endregion

        #region Subclasses

        protected class RegistrationEntry
        {
            public WeakReference Subscriber { get; set; }
            public int PublisherHashCode { get; set; }
            public Type MessageType { get; set; }
            public bool OrDerived { get; set; }
        }

        #endregion
    }

    //constrains the interface to a single generic notify method
    internal class GenericLightMessageBus<T> : IMessages<T> where T : IMessage
    {
        #region Constructors

        //hide default constructor to force the use of instance property
        private GenericLightMessageBus() { }

        #endregion

        #region Singleton

        private static readonly Lazy<GenericLightMessageBus<T>> DefaultInstance = new Lazy<GenericLightMessageBus<T>>(()=>new GenericLightMessageBus<T>());

        public static GenericLightMessageBus<T> Default
        {
            get { return DefaultInstance.Value; }
        }

        #endregion

        #region IMessages

        public IMessages<T> OrDerived()
        {
            ((MessageBus)MessageBus.Default).OrDerived();

            return this;
        }

        public void Notify(IMessageHandler<T> subscriber)
        {
            ((IMessages) MessageBus.Default).Notify(subscriber);
        }

        #endregion
    }
}
