namespace LightMessageBus.Test.TestClasses
{
    public class BetaMessage : MessageWithSource
    {
        public BetaMessage(object publisher) : base(publisher)
        {}
    }
}