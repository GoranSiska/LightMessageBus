namespace LightMessageBus.Interfaces
{
    public interface IMessage
    {
        object Source { get; set; }
    }
}