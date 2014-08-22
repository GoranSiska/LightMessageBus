namespace LightMessageBus.Test.TestClasses
{
    public class CommonPublisher
    {
        public MessageWithSource Message()
        {
            return new MessageWithSource(this);
        }
    }
}