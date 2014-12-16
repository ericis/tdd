namespace Archient.DesignPatterns.Gumball.Tests.Features
{
    using System;
    using Archient.DesignPatterns.Gumball.Client;
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Output;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class StepDefinitions : IDisposable
    {
        private readonly IGumballHardware hardware;
        
        private readonly IDisposable eventsSubscription;

        private IGumballMachine gumballMachine;

        private bool receivedReturnQuarterEvent;

        private string lastDisplayMessage;

        public StepDefinitions()
        {
            this.hardware = new HardwareTestDouble();

            this.eventsSubscription = this.hardware.Subscribe(this.TrackEventChanges);
        }

        [Given(@"I have a new gumball machine")]
        public void GivenIHaveANewGumballMachine()
        {
            this.gumballMachine = GumballMachineFactory.Create(hardware);
        }

        [When(@"I refill it")]
        public void WhenIRefillIt()
        {
            this.hardware.OnNext(new RefillGumballsEvent());
        }

        [When(@"I insert a quarter")]
        public void WhenIInsertAQuarter()
        {
            this.receivedReturnQuarterEvent = false;

            this.hardware.OnNext(new InsertQuarterEvent());
        }

        [When(@"I turn the crank")]
        public void WhenITurnTheCrank()
        {
            this.hardware.OnNext(new TurnCrankEvent());
        }

        [When(@"a gumball is dispensed")]
        public void WhenAGumballIsDispensed()
        {
            this.hardware.OnNext(new GumballDispensedEvent());
        }

        [When(@"a gumball is not dispensed")]
        public void WhenAGumballIsNotDispensed()
        {
            this.hardware.OnNext(new OutOfGumballsEvent());
        }

        [When(@"I eject a quarter")]
        public void WhenIEjectAQuarter()
        {
            this.hardware.OnNext(new EjectQuarterEvent());
        }

        [When(@"I try to take a gumball")]
        public void WhenITryToTakeAGumball()
        {
            this.hardware.OnNext(new TakeGumballEvent());
        }

        [Then(@"the display reads ""(.*)""")]
        public void ThenTheDisplayReads(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, this.lastDisplayMessage);
        }

        [Then(@"it returns my quarter")]
        public void ThenItReturnsMyQuarter()
        {
            Assert.IsTrue(this.receivedReturnQuarterEvent);
        }

        public void Dispose()
        {
            this.eventsSubscription.Dispose();
        }

        private void TrackEventChanges(GumballEvent gumballEvent)
        {
            // check for 'return quarter'
            if (gumballEvent is ReturnQuarterEvent)
            {
                this.receivedReturnQuarterEvent = true;
                return;
            }

            var displayMessageEvent = gumballEvent as DisplayMessageEvent;

            // skip any non-display message event
            if (displayMessageEvent == null) return;

            this.lastDisplayMessage = displayMessageEvent.Value;
        }
    }
}