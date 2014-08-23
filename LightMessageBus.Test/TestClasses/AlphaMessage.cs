namespace LightMessageBus.Test.TestClasses
{
    public class AlphaMessage : MessageWithSource
    {
        public AlphaMessage(object publisher) : base(publisher)
        {}
    }
}