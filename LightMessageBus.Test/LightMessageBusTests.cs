using LightMessageBus.Interfaces;
using LightMessageBus.Test.TestClasses;
using NUnit.Framework;

namespace LightMessageBus.Test
{
    [TestFixture]
    public class LightMessageBusTests
    {
        #region Default
        
        [Test]
        public void GivenMessageBus_WhenDefault_ReturnsObject()
        {
            Assert.IsNotNull(LightMessageBus.Default);
        }

        [Test]
        public void GivenMessageBus_WhenDefault_ReturnsSameInstance()
        {
            Assert.AreSame(LightMessageBus.Default, LightMessageBus.Default);
        }

        [Test]
        public void GivenMessageBus_WhenDefault_ReturnsPublishers()
        {
            Assert.IsInstanceOf<IPublishers>(LightMessageBus.Default);
        }

        #endregion

        #region From

        [Test]
        public void GivenMessageBus_WhenFrom_ReturnsMessages()
        {
            var messages = LightMessageBus.Default.From(new object());

            Assert.IsInstanceOf<IMessages>(messages);
        }

        #endregion

        #region Notify

        [Test]
        public void GivenSubscriber_WhenSubscriberRegistered_SubscriberRegistered()
        {
            var subscriber = new EmptySubscriber();

            LightMessageBus.Default.From(new object()).Notify(subscriber);

            Assert.IsTrue(LightMessageBus.Default.HasRegistered(subscriber));
        }

        [Test]
        public void GivenSubscriber_WhenSubscriberNotRegistered_SubscriberNotRegistered()
        {
            var subscriber = new EmptySubscriber();

            Assert.IsFalse(LightMessageBus.Default.HasRegistered(subscriber));
        }

        #endregion

        #region Publish

        [Test]
        public void GivenRegisteredSubscriber_WhenMessagePublished_SubscriberNotified()
        {
            var subscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(new object()).Notify(subscriber);

            LightMessageBus.Default.Publish(new object());

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenRegisteredSubscriber_WhenMessagePublished_OtherSubscriberNotNotified()
        {
            var otherSubscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(new object()).Notify(new NotifiableSubscriber());

            LightMessageBus.Default.Publish(new object());

            Assert.IsFalse(otherSubscriber.IsNotified);
        }

        [Test]
        public void GivenMultipleRegisteredSubscribers_WhenMessagePublished_AllSubscribersNotified()
        {
            var publisher = new object();
            var firstSubscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(publisher).Notify(firstSubscriber);
            var secondSubscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(publisher).Notify(secondSubscriber);

            LightMessageBus.Default.Publish(new object());

            Assert.IsTrue(firstSubscriber.IsNotified);
            Assert.IsTrue(secondSubscriber.IsNotified);
        }

        #endregion

        
    }
}
