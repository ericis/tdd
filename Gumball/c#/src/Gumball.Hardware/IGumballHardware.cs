using System;
using Archient.DesignPatterns.Gumball.Hardware.Events;
namespace Archient.DesignPatterns.Gumball.Hardware
{
    /**********************************************************
    *
    * ---------------------------------
    *  Gumball Hardware Specification
    * ---------------------------------
    * 
    *   The hardware specification is owned and maintained by 
    *   the hardware team and is imported by the gumball 
    *   machine software team.  The software team is 
    *   responsible for integrating the API of the device with 
    *   the actual hardware.
    *
    **********************************************************/

    /// <summary>
    /// Contract for gumball hardware
    /// </summary>
    public interface IGumballHardware :
        // can raise internal events that others can listen to
        IObservable<GumballEvent>,
        // can observe external events
        IObserver<GumballEvent>
    {
    }
}
