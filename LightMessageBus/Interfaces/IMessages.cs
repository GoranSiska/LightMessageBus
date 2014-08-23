namespace LightMessageBus.Interfaces
{
    public interface IMessages
    {
        IMessages Where<T>() where T : IMessage;
        void Notify(IMessageHandler subscriber);
    }
}