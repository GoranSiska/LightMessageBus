namespace LightMessageBus.Interfaces
{
    public interface IPublishers
    {
        IMessages From(object publisher);
        void Publish(IMessage message);

        bool HasRegistered(IMessageHandler subscriber);
    }
}