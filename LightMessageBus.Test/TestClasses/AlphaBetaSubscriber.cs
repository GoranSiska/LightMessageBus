using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class AlphaBetaSubscriber : IMessageHandler<AlphaMessage>, IMessageHandler<BetaMessage>
    {
        public bool AlphaMessageHandled { get; private set; }
        public void Handle(AlphaMessage message)
        {
            AlphaMessageHandled = true;
        }

        public bool BetaMessageHandled { get; private set; }
        public void Handle(BetaMessage message)
        {
            BetaMessageHandled = true;
        }
    }
}