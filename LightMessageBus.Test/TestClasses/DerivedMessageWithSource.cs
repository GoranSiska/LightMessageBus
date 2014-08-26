namespace LightMessageBus.Test.TestClasses
{
    public class DerivedMessageWithSource : MessageWithSource
    {
        public DerivedMessageWithSource(object publisher) : base(publisher)
        {}
    }
}