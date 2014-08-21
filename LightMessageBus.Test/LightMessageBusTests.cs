using LightMessageBus.Interfaces;
using NUnit.Framework;

namespace LightMessageBus.Test
{
    [TestFixture]
    public class LightMessageBusTests
    {
        #region Singleton
        
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

        #endregion

        #region IMessages

        [Test]
        public void From_ReturnsUntypedMessages()
        {
            var messages = LightMessageBus.Default.From(new object());

            Assert.IsInstanceOf<IMessages>(messages);
        }

        #endregion
    }
}
