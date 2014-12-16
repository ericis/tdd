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

/// Base class raised whenever a gumball hardware event occurs.
[<AbstractClass>]
type GumballEvent = 
    new : unit -> GumballEvent
    
/// <summary>Generic base class raised whenever a gumball hardware event occurs.</summary>
/// <typeparam name="t">The type of argument passed in the event</typeparam>
[<AbstractClass>]
type GumballEvent<'t> = 
    inherit GumballEvent
    new : 't -> GumballEvent<'t>
    
    /// Gets the argument value passed in the event
    member Value : 't with get

/// All hardware inputs received from outside sources
module Input =
    
    /// Raised to request that the hardware display a message
    [<Sealed>]
    type DisplayMessageEvent =
        inherit GumballEvent<string>
        new : string -> DisplayMessageEvent
    
    /// Raised to request that the hardware return a quarter
    [<Sealed>]
    type ReturnQuarterEvent =
        inherit GumballEvent
        new : unit -> ReturnQuarterEvent
        
/// <summary>All hardware outputs originating from the hardware</summary>
/// <remarks>Automation tests will have to use these events explicitly to simulate external hardware events</summary>
module Output =

    /// Raised from the hardware when a quarter is inserted
    [<Sealed>]
    type InsertQuarterEvent =
        inherit GumballEvent
        new : unit -> InsertQuarterEvent
        
    /// Raised from the hardware when the 'eject quarter' button is pressed
    [<Sealed>]
    type EjectQuarterEvent =
        inherit GumballEvent
        new : unit -> EjectQuarterEvent
        
    /// Raised from the hardware when the 'refill' button is pressed
    [<Sealed>]
    type RefillGumballsEvent =
        inherit GumballEvent
        new : unit -> RefillGumballsEvent
        
    /// Raised from the hardware when the gumball 'crank' is turned
    [<Sealed>]
    type TurnCrankEvent =
        inherit GumballEvent
        new : unit -> TurnCrankEvent
        
    /// Raised from the hardware when a gumball is dispensed for pick up
    [<Sealed>]
    type GumballDispensedEvent =
        inherit GumballEvent
        new : unit -> GumballDispensedEvent
        
    /// Raised from the hardware when it detects there are no more gumballs that can be dispensed
    [<Sealed>]
    type OutOfGumballsEvent =
        inherit GumballEvent
        new : unit -> OutOfGumballsEvent
        
    /// Raised from the hardware when the gumball door is opened
    [<Sealed>]
    type TakeGumballEvent =
        inherit GumballEvent
        new : unit -> TakeGumballEvent