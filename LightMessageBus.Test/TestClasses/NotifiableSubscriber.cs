using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class NotifiableSubscriber : IMessageHandler
    {
        public bool IsNotified { get; private set; }

        public void Handle(object message)
        {
            IsNotified = true;
        }
    }
}