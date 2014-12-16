namespace Archient.DesignPatterns.Gumball.Domain.State
{
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Output;
    using Archient.DesignPatterns.Gumball.Properties;

    internal class SoldOutGumballEventListener : GumballEventListenerBase
    {
        public SoldOutGumballEventListener(IGumballHardware hardware)
            : base(hardware)
        {
        }

        public override void Initialize()
        {
            this.DisplayMessage(Resources.SoldOut_Start);
        }

        public override IGumballEventListener OnEvent(GumballEvent gumballEvent)
        {
            if (gumballEvent is RefillGumballsEvent)
            {
                this.DisplayMessage(Resources.Ready_Start);
                
                // switch to 'ready for quarter' state
                return new ReadyForQuarterGumballEventListener(this.hardware);
            }

            if (gumballEvent is InsertQuarterEvent)
            {
                this.RaiseEvent<ReturnQuarterEvent>();

                this.DisplayMessage(Resources.SoldOut_Quarter);
            }
            else if (gumballEvent is EjectQuarterEvent)
            {
                this.DisplayMessage(Resources.SoldOut_Eject);
            }
            else if (gumballEvent is TurnCrankEvent)
            {
                this.DisplayMessage(Resources.SoldOut_Crank);
            }
            else if (gumballEvent is TakeGumballEvent)
            {
                this.DisplayMessage(Resources.SoldOut_Take);
            }

            return this;
        }
    }
}