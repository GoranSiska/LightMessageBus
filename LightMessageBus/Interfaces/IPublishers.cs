namespace LightMessageBus.Interfaces
{
    public interface IPublishers
    {
        IMessages From(object publisher);
        IMessages FromAny();
        void Publish<T>(T message) where T : IMessage;

        bool HasRegistered<T>(IMessageHandler<T> subscriber) where T : IMessage;
    }
}