namespace LightMessageBus.Interfaces
{
    public interface IPublishers
    {
        IMessages From(object publisher);
        void Publish(object message);

        bool HasRegistered(IMessageHandler subscriber);
    }
}