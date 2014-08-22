using System;
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

        private IMessageHandler _registeredSubscriber;

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
            _registeredSubscriber.Handle(message);
        }

        public bool HasRegistered(object subscriber)
        {
            return subscriber == _registeredSubscriber;
        }

        #endregion

        #region IMessages

        public void Notify(IMessageHandler subscriber)
        {
            _registeredSubscriber = subscriber;
        }

        #endregion


    }
}
