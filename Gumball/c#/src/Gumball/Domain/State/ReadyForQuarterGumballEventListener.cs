namespace Archient.DesignPatterns.Gumball.Domain.State
{
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Output;
    using Archient.DesignPatterns.Gumball.Properties;

    internal class ReadyForQuarterGumballEventListener : GumballEventListenerBase
    {
        public ReadyForQuarterGumballEventListener(IGumballHardware hardware)
            : base(hardware)
        {
        }

        public override IGumballEventListener OnEvent(GumballEvent gumballEvent)
        {
            if (gumballEvent is InsertQuarterEvent)
            {
                this.DisplayMessage(Resources.Ready_Quarter);
                
                // switch to 'has quarter' state
                return new HasQuarterGumballEventListener(this.hardware);
            }

            if (gumballEvent is EjectQuarterEvent)
            {
                this.DisplayMessage(Resources.Ready_Eject);
            }
            else if (gumballEvent is TurnCrankEvent)
            {
                this.DisplayMessage(Resources.Ready_Crank);
            }
            else if (gumballEvent is TakeGumballEvent)
            {
                this.DisplayMessage(Resources.Ready_Take);
            }

            return this;
        }
    }
}