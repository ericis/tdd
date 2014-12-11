namespace Archient.DesignPatterns.Gumball.Hardware.Events

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

/// Implementation of an abstract Gumball Event
[<AbstractClass>]
type GumballEvent = 
    new : unit -> GumballEvent

/// Hardware input events
module Input =
    
    /// Event raised to request the hardware to return the quarter
    [<Sealed>]
    type ReturnQuarterEvent =
        inherit GumballEvent
        new : unit -> ReturnQuarterEvent

/// Hardware output events
module Output =

    /// Event raised when a quarter is inserted
    [<Sealed>]
    type InsertQuarterEvent =
        inherit GumballEvent
        new : unit -> InsertQuarterEvent

    /// Event raised when the eject quarter button is pressed
    [<Sealed>]
    type EjectQuarterEvent =
        inherit GumballEvent
        new : unit -> EjectQuarterEvent

    /// Event raised when the refill gumballs button is pressed
    [<Sealed>]
    type RefillGumballsEvent =
        inherit GumballEvent
        new : unit -> RefillGumballsEvent

    /// Event raised when the crank is turned
    [<Sealed>]
    type TurnCrankEvent =
        inherit GumballEvent
        new : unit -> TurnCrankEvent

    /// Event raised when a gumball is dispensed
    [<Sealed>]
    type GumballDispensedEvent =
        inherit GumballEvent
        new : unit -> GumballDispensedEvent

    /// Event raised when the machine is out of gumballs
    [<Sealed>]
    type OutOfGumballsEvent =
        inherit GumballEvent
        new : unit -> OutOfGumballsEvent

    /// Event raised when gumballs are waiting for pick-up
    [<Sealed>]
    type TakeGumballEvent =
        inherit GumballEvent
        new : unit -> TakeGumballEvent