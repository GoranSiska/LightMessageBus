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

        #region IMessages

        public IMessages From(object publisher)
        {
            return this;
        }

        public void Notify(object subscriber)
        {
            (subscriber as dynamic).IsRegistered = true;
        }

        #endregion


    }
}
