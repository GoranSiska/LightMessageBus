namespace LightMessageBus.Interfaces
{
    public interface IMessages
    {
        IMessages Where<T>() where T : IMessage;
        void Notify<T>(IMessageHandler<T> subscriber) where T : IMessage;
    }
}