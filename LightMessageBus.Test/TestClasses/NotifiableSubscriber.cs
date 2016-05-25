using System;
using LightMessageBus.Interfaces;

namespace LightMessageBus.Test.TestClasses
{
    public class NotifiableSubscriber : IMessageHandler<MessageWithSource>
    {
        public bool IsNotified { get; private set; }
        public event EventHandler Handled;

        public void Handle(MessageWithSource message)
        {
            IsNotified = true;
            OnHandled(EventArgs.Empty);
        }

        protected virtual void OnHandled(EventArgs e)
        {
            Handled?.Invoke(this, e);
        }
    }
}