namespace LightMessageBus.Interfaces
{
    public interface IMessages
    {
        IMessages<T> Where<T>() where T : IMessage;
        void Notify<T>(IMessageHandler<T> subscriber) where T : IMessage;
    }

    public interface IMessages<T> where T : IMessage
    {
        void Notify(IMessageHandler<T> subscriber);
    }
}