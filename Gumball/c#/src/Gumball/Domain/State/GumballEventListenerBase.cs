namespace Archient.DesignPatterns.Gumball.Domain.State
{
    using System;
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Properties;

    internal abstract class GumballEventListenerBase : IGumballEventListener
    {
        protected readonly IGumballHardware hardware;

        public GumballEventListenerBase(IGumballHardware hardware)
        {
            if (hardware == null) throw new ArgumentNullException("hardware");

            this.hardware = hardware;
        }

        public virtual void Initialize()
        {
            // no-op
        }

        public abstract IGumballEventListener OnEvent(GumballEvent gumballEvent);

        protected void RaiseEvent<TEvent>(TEvent gumballEvent)
            where TEvent : GumballEvent
        {
            this.hardware.OnNext(gumballEvent);
        }

        protected void RaiseEvent<TEvent>() 
            where TEvent : GumballEvent, new()
        {
            var gumballEvent = new TEvent();

            this.RaiseEvent(gumballEvent);
        }

        protected void DisplayMessage(string message)
        {
            this.RaiseEvent(new DisplayMessageEvent(message));
        }
    }
}