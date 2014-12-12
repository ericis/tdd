namespace Archient.DesignPatterns.Gumball

module GumballMachine =
    
    open System
    open System.ComponentModel
    open System.Diagnostics

    open Archient.DesignPatterns.Gumball.Hardware
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    type private IGumballEventHandler =
        abstract member OnEvent : event:GumballEvent -> IGumballEventHandler

    let private getCrankTurnedHandler (soldOutHandler:IGumballEventHandler) (readyHandler:IGumballEventHandler) (notify:GumballEvent->unit) =
        
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        notify (Input.ReturnQuarterEvent())
                        notify (Input.DisplayMessageEvent(Messages.Crank.Quarter))
                        me
                    | :? EjectQuarterEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Crank.Eject))
                        me
                    | :? TurnCrankEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Crank.Crank))
                        me
                    | :? OutOfGumballsEvent ->
                        notify (Input.ReturnQuarterEvent())
                        notify (Input.DisplayMessageEvent(Messages.SoldOut.Quarter))
                        soldOutHandler
                    | :? TakeGumballEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Ready.Start))
                        readyHandler
                    | _ -> me }

    let private getHasQuarterHandler (soldOutHandler:IGumballEventHandler) (readyHandler:IGumballEventHandler) (notify:GumballEvent->unit) =
        
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        notify (Input.ReturnQuarterEvent())
                        notify (Input.DisplayMessageEvent(Messages.Quarter.Quarter))
                        me
                    | :? EjectQuarterEvent ->
                        notify (Input.ReturnQuarterEvent())
                        notify (Input.DisplayMessageEvent(Messages.Quarter.Eject))
                        readyHandler
                    | :? TurnCrankEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Quarter.Crank))
                        getCrankTurnedHandler soldOutHandler readyHandler notify
                    | :? TakeGumballEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Quarter.Take))
                        me
                    | _ -> me }

    let private getReadyForQuarterHandler (soldOutHandler:IGumballEventHandler) (notify:GumballEvent->unit) =
        
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Ready.Quarter))
                        getHasQuarterHandler soldOutHandler me notify
                    | :? EjectQuarterEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Ready.Eject))
                        me
                    | :? TurnCrankEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Ready.Crank))
                        me
                    | :? TakeGumballEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Ready.Take))
                        me
                    | _ -> me }

    let private getSoldOutHandler (notify:GumballEvent->unit) =
        
        {
            new IGumballEventHandler with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? RefillGumballsEvent ->
                        notify (Input.DisplayMessageEvent(Messages.Ready.Start))
                        getReadyForQuarterHandler me notify
                    | :? InsertQuarterEvent ->
                        notify (Input.ReturnQuarterEvent())
                        notify (Input.DisplayMessageEvent(Messages.SoldOut.Quarter))
                        me
                    | :? EjectQuarterEvent ->
                        notify (Input.DisplayMessageEvent(Messages.SoldOut.Eject))
                        me
                    | :? TurnCrankEvent ->
                        notify (Input.DisplayMessageEvent(Messages.SoldOut.Crank))
                        me
                    | :? TakeGumballEvent ->
                        notify (Input.DisplayMessageEvent(Messages.SoldOut.Take))
                        me
                    | _ -> me }

    let create (hardware:IGumballHardware) =
        
        // create a default gumball machine state
        let state = ref (getSoldOutHandler hardware.OnNext)

        // listen to hardware events
        // mutate the state whenever on known hardware event
        let subscription =
            hardware.Subscribe(fun e -> state := state.Value.OnEvent(e))
        
        hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Start))

        {
            new IGumballMachine with
                override me.Hardware = hardware
                override me.Dispose() = subscription.Dispose() }