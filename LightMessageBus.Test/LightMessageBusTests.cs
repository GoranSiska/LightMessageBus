using LightMessageBus.Interfaces;
using LightMessageBus.Test.TestClasses;
using NUnit.Framework;

namespace LightMessageBus.Test
{
    [TestFixture]
    public class LightMessageBusTests
    {
        #region Singleton
        
        [Test]
        public void Default_ReturnsSingleton()
        {
            Assert.IsNotNull(LightMessageBus.Default);
        }

        [Test]
        public void Default_ReturnsSameInstance()
        {
            Assert.AreSame(LightMessageBus.Default, LightMessageBus.Default);
        }

        #endregion

        #region From

        [Test]
        public void From_ReturnsUntypedMessages()
        {
            var messages = LightMessageBus.Default.From(new object());

            Assert.IsInstanceOf<IMessages>(messages);
        }

        #endregion

        #region Notify

        [Test]
        public void Notify_SubscriberRegistered()
        {
            var subscriber = new EmptySubscriber();

            LightMessageBus.Default.From(new object()).Notify(subscriber);

            Assert.IsTrue(LightMessageBus.Default.HasRegistered(subscriber));
        }

        [Test]
        public void NoNotify_SubscriberNotRegistered()
        {
            var subscriber = new EmptySubscriber();

            Assert.IsFalse(LightMessageBus.Default.HasRegistered(subscriber));
        }

        #endregion



        
    }
}
