using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class DerivedNotifiableSubscriber : IMessageHandler<DerivedMessageWithSource>
    {
        public bool IsNotified { get; private set; }

        public void Handle(DerivedMessageWithSource message)
        {
            IsNotified = true;
        }
    }
}