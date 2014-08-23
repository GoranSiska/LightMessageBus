namespace LightMessageBus.Interfaces
{
    public interface IPublishers
    {
        IMessages From(object publisher);
        IMessages FromAny();
        void Publish(IMessage message);

        bool HasRegistered(IMessageHandler subscriber);
    }
}