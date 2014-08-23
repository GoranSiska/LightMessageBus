namespace LightMessageBus.Interfaces
{
    public interface IMessages
    {
        void Notify(IMessageHandler subscriber);
    }
}