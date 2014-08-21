namespace LightMessageBus
{
    /// <summary>
    /// Light-weight message bus
    /// </summary>
    /// <remarks>
    /// Used to enable communication between objects without direct references.
    /// </remarks>
    public class LightMessageBus
    {
        public static LightMessageBus Default
        {
            get { return new LightMessageBus(); }
        }
    }
}
