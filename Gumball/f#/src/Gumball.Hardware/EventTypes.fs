namespace Archient.DesignPatterns.Gumball.Hardware.Events

[<AbstractClass>]
type GumballEvent() = class end

module Input =
    [<Sealed>]
    type ReturnQuarterEvent() =
        inherit GumballEvent()

module Output =
    [<Sealed>]
    type InsertQuarterEvent() =
        inherit GumballEvent()
            
    [<Sealed>]
    type EjectQuarterEvent() =
        inherit GumballEvent()
            
    [<Sealed>]
    type RefillGumballsEvent() =
        inherit GumballEvent()
            
    [<Sealed>]
    type TurnCrankEvent() =
        inherit GumballEvent()
            
    [<Sealed>]
    type GumballDispensedEvent() =
        inherit GumballEvent()
            
    [<Sealed>]
    type OutOfGumballsEvent() =
        inherit GumballEvent()
            
    [<Sealed>]
    type TakeGumballEvent() =
        inherit GumballEvent()