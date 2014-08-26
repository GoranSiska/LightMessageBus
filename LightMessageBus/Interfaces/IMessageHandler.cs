namespace LightMessageBus.Interfaces
{
    public interface IMessageHandler<in T> where T : IMessage
    {
        void Handle(T message);
    }
}
