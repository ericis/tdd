namespace Archient.DesignPatterns.Gumball.Hardware

(**********************************************************
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
**********************************************************)

open System

open Archient.DesignPatterns.Gumball.Hardware.Events

/// Contract for gumball hardware
[<Interface>]
type IGumballHardware =
    
    // can raise the events from the inside
    inherit IObservable<GumballEvent>
    
    // can observe the events from the outside
    inherit IObserver<GumballEvent>