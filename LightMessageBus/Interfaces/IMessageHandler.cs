namespace LightMessageBus.Interfaces
{
    public interface IMessageHandler
    {
        void Handle(object message);
    }
}
