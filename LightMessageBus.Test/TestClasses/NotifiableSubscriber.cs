namespace LightMessageBus.Test.TestClasses
{
    public class NotifiableSubscriber
    {
        public bool IsNotified { get; private set; }

        public void Handle(object message)
        {
            IsNotified = true;
        }
    }
}