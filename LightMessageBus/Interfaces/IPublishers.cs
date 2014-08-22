namespace LightMessageBus.Interfaces
{
    public interface IPublishers
    {
        IMessages From(object publisher);
    }
}