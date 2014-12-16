namespace Archient.DesignPatterns.Gumball.Domain
{
    using System;
    using Archient.DesignPatterns.Gumball.Client;
    using Archient.DesignPatterns.Gumball.Domain.State;
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Properties;

    internal class GumballMachine : IGumballMachine
    {
        private readonly IDisposable eventsSubscription;

        private IGumballEventListener eventListener;

        public GumballMachine(IGumballHardware hardware)
        {
            if (hardware == null) throw new ArgumentNullException("hardware");

            this.Hardware = hardware;

            this.eventListener = new SoldOutGumballEventListener(this.Hardware);

            this.eventsSubscription = this.Hardware.Subscribe(this.TrackEventChanges);
        }

        public void Initialize()
        {
            this.eventListener.Initialize();
        }

        public IGumballHardware Hardware { get; private set; }

        public void Dispose()
        {
            this.eventsSubscription.Dispose();
        }

        private void TrackEventChanges(GumballEvent gumballEvent)
        {
            this.eventListener = this.eventListener.OnEvent(gumballEvent);
        }
    }
}