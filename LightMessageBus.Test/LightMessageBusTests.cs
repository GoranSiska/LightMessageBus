using NUnit.Framework;

namespace LightMessageBus.Test
{
    [TestFixture]
    public class LightMessageBusTests
    {
        [Test]
        public void Default_ReturnsSingleton()
        {
            Assert.IsNotNull(LightMessageBus.Default);
        }

        [Test]
        public void Default_ReturnsSameInstance()
        {
            Assert.AreSame(LightMessageBus.Default, LightMessageBus.Default);
        }
    }
}
