namespace Archient.DesignPatterns.Gumball.Domain.State
{
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Input;
    using Archient.DesignPatterns.Gumball.Hardware.Events.Output;
    using Archient.DesignPatterns.Gumball.Properties;

    internal class CrankTurnedGumballEventListener : GumballEventListenerBase
    {
        public CrankTurnedGumballEventListener(IGumballHardware hardware)
            : base(hardware)
        {
        }

        public override IGumballEventListener OnEvent(GumballEvent gumballEvent)
        {
            if (gumballEvent is OutOfGumballsEvent)
            {
                // return quarter, sold out
                this.RaiseEvent<ReturnQuarterEvent>();

                this.DisplayMessage(Resources.SoldOut_Quarter);
                
                // return to sold out state
                return new SoldOutGumballEventListener(this.hardware);
            }

            if (gumballEvent is TakeGumballEvent)
            {
                this.DisplayMessage(Resources.Ready_Start);

                // return to ready to sell gumballs state
                return new ReadyForQuarterGumballEventListener(this.hardware);
            }

            if (gumballEvent is InsertQuarterEvent)
            {
                // return quarter, gumball on its way
                this.RaiseEvent<ReturnQuarterEvent>();

                this.DisplayMessage(Resources.Crank_Quarter);
            }
            else if (gumballEvent is EjectQuarterEvent)
            {
                this.DisplayMessage(Resources.Crank_Eject);
            }
            else if (gumballEvent is TurnCrankEvent)
            {
                this.DisplayMessage(Resources.Crank_Crank);
            }

            return this;
        }
    }
}