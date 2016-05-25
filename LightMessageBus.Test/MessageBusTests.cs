using System;
using LightMessageBus.Interfaces;
using LightMessageBus.Test.TestClasses;
using NUnit.Framework;

namespace LightMessageBus.Test
{
    [TestFixture]
    public class MessageBusTests
    {
        #region Default
        
        [Test]
        public void GivenMessageBus_WhenDefault_ReturnsObject()
        {
            Assert.IsNotNull(MessageBus.Default);
        }

        [Test]
        public void GivenMessageBus_WhenDefault_ReturnsSameInstance()
        {
            Assert.AreSame(MessageBus.Default, MessageBus.Default);
        }

        [Test]
        public void GivenMessageBus_WhenDefault_ReturnsPublishers()
        {
            Assert.IsInstanceOf<IPublishers>(MessageBus.Default);
        }

        #endregion

        #region Where

        [Test]
        public void GivenMessageBus_WhenWhere_ReturnsTypedMessages()
        {
            Assert.IsInstanceOf<IMessages<MessageWithSource>>(MessageBus.Default.FromAny().Where<MessageWithSource>());
        }

        #endregion

        #region From

        [Test]
        public void GivenMessageBus_WhenFrom_ReturnsMessages()
        {
            var messages = MessageBus.Default.From(new object());

            Assert.IsInstanceOf<IMessages>(messages);
        }

        [Test]
        public void GivenMessageBus_WhenFromAny_ReturnsMessages()
        {
            var messages = MessageBus.Default.FromAny();

            Assert.IsInstanceOf<IMessages>(messages);
        }

        #endregion

        #region Notify

        [Test]
        public void GivenSubscriber_WhenSubscriberRegistered_SubscriberRegistered()
        {
            var subscriber = new EmptySubscriber();

            MessageBus.Default.From(new object()).Notify(subscriber);

            Assert.IsTrue(MessageBus.Default.HasRegistered(subscriber));
        }

        [Test]
        public void GivenSubscriber_WhenSubscriberNotRegistered_SubscriberNotRegistered()
        {
            var subscriber = new EmptySubscriber();

            Assert.IsFalse(MessageBus.Default.HasRegistered(subscriber));
        }

        [Test]
        public void GiveSubscriber_WhenSubscriberDisposed_SubscriberNotReferenced()
        {
            WeakReference subscriberWeakReference = null;
            new Action(() =>
            {
                var subscriber = new NotifiableSubscriber();
                MessageBus.Default.FromAny().Notify(subscriber);

                subscriberWeakReference = new WeakReference(subscriber);
            })();          

            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Assert.IsFalse(subscriberWeakReference.IsAlive);
        }

        [Test]
        public void GiveSubscriber_WhenSubscriberDisposed_SubscriberNotRegistered()
        { }

        #endregion

        #region Publish

        [Test]
        public void GivenRegisteredSubscriber_WhenMessagePublished_SubscriberNotified()
        {
            var subscriber = new NotifiableSubscriber();
            var publisher = new CommonPublisher();
            MessageBus.Default.From(publisher).Notify(subscriber);

            MessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenRegisteredSubscriber_WhenMessagePublished_OtherSubscriberNotNotified()
        {
            var otherSubscriber = new NotifiableSubscriber();
            var publisher = new CommonPublisher();
            MessageBus.Default.From(new object()).Notify(new NotifiableSubscriber());

            MessageBus.Default.Publish(publisher.Message());

            Assert.IsFalse(otherSubscriber.IsNotified);
        }

        [Test]
        public void GivenMultipleRegisteredSubscribers_WhenMessagePublished_AllSubscribersNotified()
        {
            var publisher = new CommonPublisher();
            var firstSubscriber = new NotifiableSubscriber();
            MessageBus.Default.From(publisher).Notify(firstSubscriber);
            var secondSubscriber = new NotifiableSubscriber();
            MessageBus.Default.From(publisher).Notify(secondSubscriber);

            MessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(firstSubscriber.IsNotified && secondSubscriber.IsNotified);
        }

        [Test]
        public void GivenMultiplePublishers_WhenMessagePublished_SubscriberToOtherPublisherNotNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.From(new object()).Notify(subscriber);
            
            MessageBus.Default.Publish(publisher.Message());

            Assert.IsFalse(subscriber.IsNotified);
        }

        [Test]
        public void GivenAnyPublisher_WhenMessagePublished_SubscriberToAnyPublisherNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.FromAny().Notify(subscriber);

            MessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenSubscriptionToTypedMessage_WhenMessagePublished_SubscriberNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.FromAny().Where<MessageWithSource>().Notify(subscriber);

            MessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenSubscriptionToTypedMessage_WhenDifferentMessagePublished_SubscriberNotNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.FromAny().Where<MessageWithSource>().Notify(subscriber);

            MessageBus.Default.Publish(new MessageWithValue(publisher));

            Assert.IsFalse(subscriber.IsNotified);
        }

        [Test]
        public void GivenSubscriptionToTypedMessage_WhenBaseMessagePublished_SubscriberNotNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new DerivedNotifiableSubscriber();
            MessageBus.Default.FromAny().Where<DerivedMessageWithSource>().Notify(subscriber);

            MessageBus.Default.Publish(new MessageWithSource(publisher));

            Assert.IsFalse(subscriber.IsNotified);
        }

        [Test]
        public void GivenSubscriptionToTypedMessage_WhenDerivedMessagePublished_SubscriberNotNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.FromAny().Where<MessageWithSource>().Notify(subscriber);

            MessageBus.Default.Publish(new DerivedMessageWithSource(publisher));

            Assert.IsFalse(subscriber.IsNotified);
        }

        [Test]
        public void GivenSubscriptionToDerivedMessageWithDerived_WhenBaseMessagePublished_SubscriberNotNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new DerivedNotifiableSubscriber();
            MessageBus.Default.FromAny().Where<DerivedMessageWithSource>().OrDerived().Notify(subscriber);

            MessageBus.Default.Publish(new MessageWithSource(publisher));

            Assert.IsFalse(subscriber.IsNotified);
        }

        [Test]
        public void GivenSubscriptionToBaseMessageWithDerived_WhenDerivedMessagePublished_SubscriberNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.FromAny().Where<MessageWithSource>().OrDerived().Notify(subscriber);

            MessageBus.Default.Publish(new DerivedMessageWithSource(publisher));

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenMultipleSubscriptionToTypedMessage_WhenMessagePublished_SubscriberNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new NotifiableSubscriber();
            MessageBus.Default.FromAny().Where<MessageWithSource>().Notify(subscriber);

            MessageBus.Default.Publish(new MessageWithSource(publisher));

            Assert.IsTrue(subscriber.IsNotified);
        }

        [Test]
        public void GivenMultipleSubscriptionToTypedMessage_WhenMessagePublished_SubscriberSameHandleNotified()
        {
            var publisher = new CommonPublisher();
            var subscriber = new AlphaBetaSubscriber();
            MessageBus.Default.FromAny().Where<AlphaMessage>().Notify(subscriber);

            MessageBus.Default.Publish(new AlphaMessage(publisher));

            Assert.IsTrue(subscriber.AlphaMessageHandled);
        }

        [Test]
        public void GivenRegisteredSubscriber_WhenMessagePublishedAndWithinNewSubscriberIsCreated_DontThrow()
        {
            var subscriber = new NotifiableSubscriber();
            var subscriber2 = new NotifiableSubscriber();
            var publisher = new CommonPublisher();
            MessageBus.Default.From(publisher).Notify(subscriber);

            subscriber.Handled += (s, e) =>
            {
                MessageBus.Default.From(publisher).Notify(subscriber2);
            };
            MessageBus.Default.Publish(publisher.Message());

            Assert.IsTrue(subscriber.IsNotified);
        }
        #endregion

    }
}
