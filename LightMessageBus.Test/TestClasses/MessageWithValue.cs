namespace LightMessageBus.Test.TestClasses
{
    public class MessageWithValue : MessageWithSource
    {
        public MessageWithValue(object publisher) : base(publisher)
        {
            Value = -1;
        }

        public int Value { get; set; }
    }
}