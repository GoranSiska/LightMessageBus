namespace LightMessageBus.Interfaces
{
    public interface IMessages
    {
        void Notify(object subscriber);
    }
}