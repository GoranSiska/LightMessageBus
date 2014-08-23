using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class EmptySubscriber : IMessageHandler
    {
        public bool IsRegistered { get; set; }
        
        public void Handle(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}