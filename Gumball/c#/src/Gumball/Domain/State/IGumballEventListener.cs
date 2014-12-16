namespace Archient.DesignPatterns.Gumball.Domain.State
{
    using Archient.DesignPatterns.Gumball.Hardware.Events;

    internal interface IGumballEventListener
    {
        void Initialize();

        IGumballEventListener OnEvent(GumballEvent gumballEvent);
    }
}