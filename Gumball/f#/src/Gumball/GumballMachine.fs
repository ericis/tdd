namespace Archient.DesignPatterns.Gumball

module GumballMachine =
    
    open System
    
    open Archient.DesignPatterns.Gumball.Hardware
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    /// <summary>Internal state representation of a gumball machine.</summary>
    /// <remarks>
    /// Each gumball machine state handles gumball events and may return itself 
    /// to continue handling events or may return a new or previous state to 
    /// handle the events.
    /// </remarks>
    type private IGumballEventHandler =
        abstract member OnEvent : event:GumballEvent -> IGumballEventHandler

    let private displayMessage (notify:GumballEvent->unit) message =
        notify (Input.DisplayMessageEvent(message))

    let private getCrankTurnedHandler (soldOutHandler:IGumballEventHandler) (readyHandler:IGumballEventHandler) (notify:GumballEvent->unit) =
        
        // object expression to create an in-line instance of the interface
        // (F# compiler generates a class definition for us :)
        // http://msdn.microsoft.com/en-us/library/dd233237.aspx
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        // return quarter, gumball on its way
                        // no state change
                        notify (Input.ReturnQuarterEvent())
                        displayMessage notify Messages.Crank.Quarter
                        me
                    | :? EjectQuarterEvent ->
                        displayMessage notify Messages.Crank.Eject
                        me
                    | :? TurnCrankEvent ->
                        displayMessage notify Messages.Crank.Crank
                        me
                    | :? OutOfGumballsEvent ->
                        // return quarter, sold out
                        // return to sold out state
                        notify (Input.ReturnQuarterEvent())
                        displayMessage notify Messages.SoldOut.Quarter
                        soldOutHandler
                    | :? TakeGumballEvent ->
                        // return to ready to sell gumballs state
                        displayMessage notify Messages.Ready.Start
                        readyHandler
                    | _ -> me }

    let private getHasQuarterHandler (soldOutHandler:IGumballEventHandler) (readyHandler:IGumballEventHandler) (notify:GumballEvent->unit) =
        
        // object expression to create an in-line instance of the interface
        // (F# compiler generates a class definition for us :)
        // http://msdn.microsoft.com/en-us/library/dd233237.aspx
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        // return quarter, already has one
                        notify (Input.ReturnQuarterEvent())
                        displayMessage notify Messages.Quarter.Quarter
                        me
                    | :? EjectQuarterEvent ->
                        // return quarter, they asked for it back
                        notify (Input.ReturnQuarterEvent())
                        displayMessage notify Messages.Quarter.Eject
                        readyHandler
                    | :? TurnCrankEvent ->
                        // switch to 'crank turned' state
                        displayMessage notify Messages.Quarter.Crank
                        getCrankTurnedHandler soldOutHandler readyHandler notify
                    | :? TakeGumballEvent ->
                        displayMessage notify Messages.Quarter.Take
                        me
                    | _ -> me }

    let private getReadyForQuarterHandler (soldOutHandler:IGumballEventHandler) (notify:GumballEvent->unit) =
        
        // object expression to create an in-line instance of the interface
        // (F# compiler generates a class definition for us :)
        // http://msdn.microsoft.com/en-us/library/dd233237.aspx
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        // switch to 'has quarter' state
                        displayMessage notify Messages.Ready.Quarter
                        getHasQuarterHandler soldOutHandler me notify
                    | :? EjectQuarterEvent ->
                        displayMessage notify Messages.Ready.Eject
                        me
                    | :? TurnCrankEvent ->
                        displayMessage notify Messages.Ready.Crank
                        me
                    | :? TakeGumballEvent ->
                        displayMessage notify Messages.Ready.Take
                        me
                    | _ -> me }

    let private getSoldOutHandler (notify:GumballEvent->unit) =
        
        // object expression to create an in-line instance of the interface
        // (F# compiler generates a class definition for us :)
        // http://msdn.microsoft.com/en-us/library/dd233237.aspx
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? RefillGumballsEvent ->
                        // switch to 'ready for quarter' state
                        displayMessage notify Messages.Ready.Start
                        getReadyForQuarterHandler me notify
                    | :? InsertQuarterEvent ->
                        // return quarter, no gumballs
                        notify (Input.ReturnQuarterEvent())
                        displayMessage notify Messages.SoldOut.Quarter
                        me
                    | :? EjectQuarterEvent ->
                        displayMessage notify Messages.SoldOut.Eject
                        me
                    | :? TurnCrankEvent ->
                        displayMessage notify Messages.SoldOut.Crank
                        me
                    | :? TakeGumballEvent ->
                        displayMessage notify Messages.SoldOut.Take
                        me
                    | _ -> me }

    let create (hardware:IGumballHardware) =
        
        // create a default gumball machine state
        // start with a reference to "sold out" state
        let machineHardwareEventHandler = ref (getSoldOutHandler hardware.OnNext)

        // listen to hardware events
        // mutate the state whenever on known hardware event
        // mutation may result in the same state or a different state (up to the event handler)
        let subscription =
            hardware.Subscribe(fun e -> 
                
                // handle the event
                let hardwareEventHandler = machineHardwareEventHandler.Value.OnEvent(e)
                
                // update the machine state event handler
                // if a a new event handler was returned
                // *Could just always mutate the state, but chose to minimize mutations.
                if hardwareEventHandler <> machineHardwareEventHandler.Value then
                    machineHardwareEventHandler := hardwareEventHandler)
        
        // display sold out
        displayMessage hardware.OnNext Messages.SoldOut.Start
        
        // Gumball Machine: Public API is very limited.
        // Real-world client interactions are expected to be with the hardware events.
        // So, software clients (tests) are expected to use hardware events as well.
        // 
        // object expression to create an in-line instance of the interface
        // (F# compiler generates a class definition for us :)
        // http://msdn.microsoft.com/en-us/library/dd233237.aspx
        {
            new IGumballMachine with
                override me.Hardware = hardware
                override me.Dispose() = subscription.Dispose() }