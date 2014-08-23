using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class NotifiableSubscriber : IMessageHandler<MessageWithSource>
    {
        public bool IsNotified { get; private set; }

        public void Handle(MessageWithSource message)
        {
            IsNotified = true;
        }
    }
}