using System;
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

        [Test]
        public void GivenMessageBus_WhenFromAny_ReturnsMessages()
        {
            var messages = LightMessageBus.Default.FromAny();

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
            var publisher = new CommonPublisher();
            LightMessageBus.Default.From(publisher).Notify(subscriber);

            LightMessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenRegisteredSubscriber_WhenMessagePublished_OtherSubscriberNotNotified()
        {
            var otherSubscriber = new NotifiableSubscriber();
            var publisher = new CommonPublisher();
            LightMessageBus.Default.From(new object()).Notify(new NotifiableSubscriber());

            LightMessageBus.Default.Publish(publisher.Message());

            Assert.IsFalse(otherSubscriber.IsNotified);
        }

        [Test]
        public void GivenMultipleRegisteredSubscribers_WhenMessagePublished_AllSubscribersNotified()
        {
            var publisher = new CommonPublisher();
            var firstSubscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(publisher).Notify(firstSubscriber);
            var secondSubscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(publisher).Notify(secondSubscriber);

            LightMessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(firstSubscriber.IsNotified && secondSubscriber.IsNotified);
        }

        [Test]
        public void GivenMultiplePublishers_WhenMessagePublished_SubscriberToOtherPublisherNotNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            LightMessageBus.Default.From(new object()).Notify(subscriber);
            
            LightMessageBus.Default.Publish(publisher.Message());

            Assert.IsFalse(subscriber.IsNotified);
        }

        [Test]
        public void GivenAnyPublisher_WhenMessagePublished_SubscriberToAnyPublisherNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            LightMessageBus.Default.FromAny().Notify(subscriber);

            LightMessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(subscriber.IsNotified);
        }

        #endregion
    }
}
