using System;
using System.Collections.Generic;
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

        private IList<IMessageHandler> _registeredSubscribers = new List<IMessageHandler>();

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
            return this;
        }

        public void Publish(object message)
        {
            foreach (var registeredSubscriber in _registeredSubscribers)
            {
                registeredSubscriber.Handle(message);   
            }
        }

        public bool HasRegistered(IMessageHandler subscriber)
        {
            return _registeredSubscribers.Contains(subscriber);
        }

        #endregion

        #region IMessages

        public void Notify(IMessageHandler subscriber)
        {
            _registeredSubscribers.Add(subscriber);
        }

        #endregion


    }
}
