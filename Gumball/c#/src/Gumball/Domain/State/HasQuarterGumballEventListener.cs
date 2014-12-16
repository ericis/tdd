namespace Archient.DesignPatterns.Gumball.Domain.State
{
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Output;
    using Archient.DesignPatterns.Gumball.Properties;

    internal class HasQuarterGumballEventListener : GumballEventListenerBase
    {
        public HasQuarterGumballEventListener(IGumballHardware hardware)
            : base(hardware)
        {
        }

        public override IGumballEventListener OnEvent(GumballEvent gumballEvent)
        {
            if (gumballEvent is TurnCrankEvent)
            {
                this.DisplayMessage(Resources.Quarter_Crank);

                // switch to 'crank turned' state
                return new CrankTurnedGumballEventListener(this.hardware);
            }

            if (gumballEvent is InsertQuarterEvent)
            {
                // return quarter, already has one
                this.RaiseEvent<ReturnQuarterEvent>();

                this.DisplayMessage(Resources.Quarter_Quarter);
            }
            else if (gumballEvent is EjectQuarterEvent)
            {
                // return quarter, they asked for it back
                this.RaiseEvent<ReturnQuarterEvent>();

                this.DisplayMessage(Resources.Quarter_Eject);
            }
            else if (gumballEvent is TakeGumballEvent)
            {
                this.DisplayMessage(Resources.Quarter_Take);
            }

            return this;
        }
    }
}