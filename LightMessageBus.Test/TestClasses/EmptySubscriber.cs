using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class EmptySubscriber : IMessageHandler<MessageWithValue>
    {
        public bool IsRegistered { get; set; }

        public void Handle(MessageWithValue message)
        {
            throw new System.NotImplementedException();
        }
    }
}