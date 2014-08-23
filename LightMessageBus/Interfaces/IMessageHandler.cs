namespace LightMessageBus.Interfaces
{
    public interface IMessageHandler<T> where T : IMessage
    {
        void Handle(T message);
    }
}
