using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class MessageWithSource : IMessage
    {
        public MessageWithSource(object publisher)
        {
            Source = publisher;
        }

        public object Source { get; set; }
    }
}