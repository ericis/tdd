namespace Archient.DesignPatterns.Gumball

module GumballMachine =
    
    open System
    open System.ComponentModel
    open System.Diagnostics

    open Archient.DesignPatterns.Gumball.Hardware
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    type private IGumballMachineState =
        
        abstract member Hardware : IGumballHardware with get
        
        abstract member OnEvent : event:GumballEvent -> IGumballMachineState

    type private DefaultGumballMachineState(hardware:IGumballHardware) =
        
        new(state:IGumballMachineState) = DefaultGumballMachineState(state.Hardware)
        
        member me.Hardware = hardware
        
        abstract member OnEvent : event:GumballEvent -> IGumballMachineState
        default me.OnEvent(event:GumballEvent) = me :> IGumballMachineState

        interface IGumballMachineState with
            override me.Hardware = me.Hardware

            override me.OnEvent(event) =  me.OnEvent(event)

    let private getDoNothingState (state:IGumballMachineState) =
        new DefaultGumballMachineState(state)
        :> IGumballMachineState

    let private getCrankTurnedState (readyState:IGumballMachineState) (hasQuarterState:IGumballMachineState) =
        
        {
            new DefaultGumballMachineState(hasQuarterState) with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        me.Hardware.OnNext(Input.ReturnQuarterEvent())
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Crank.Quarter))
                        me :> IGumballMachineState
                    | :? EjectQuarterEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Crank.Eject))
                        me :> IGumballMachineState
                    | :? TurnCrankEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Crank.Crank))
                        me :> IGumballMachineState
                    | :? OutOfGumballsEvent ->
                        me.Hardware.OnNext(Input.ReturnQuarterEvent())
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Quarter))
                        readyState
                    | :? TakeGumballEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Start))
                        readyState
                    | _ -> 
                        base.OnEvent(event) }
        :> IGumballMachineState

    let private getHasQuarterState (readyState:IGumballMachineState) =
        
        {
            new DefaultGumballMachineState(readyState) with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        me.Hardware.OnNext(Input.ReturnQuarterEvent())
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Quarter.Quarter))
                        me :> IGumballMachineState
                    | :? EjectQuarterEvent ->
                        me.Hardware.OnNext(Input.ReturnQuarterEvent())
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Quarter.Eject))
                        readyState
                    | :? TurnCrankEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Quarter.Crank))
                        getCrankTurnedState readyState me
                    | :? TakeGumballEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Quarter.Take))
                        me :> IGumballMachineState
                    | _ -> 
                        base.OnEvent(event) }
        :> IGumballMachineState

    let private getReadyForQuarterState (state:IGumballMachineState) =
        
        {
            new DefaultGumballMachineState(state) with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? InsertQuarterEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Quarter))
                        getHasQuarterState me
                    | :? EjectQuarterEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Eject))
                        me :> IGumballMachineState
                    | :? TurnCrankEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Crank))
                        me :> IGumballMachineState
                    | :? TakeGumballEvent ->
                        me.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Take))
                        me :> IGumballMachineState
                    | _ -> 
                        base.OnEvent(event) }
        :> IGumballMachineState

    let private getSoldOutState (state:IGumballMachineState) =
        
        {
            new DefaultGumballMachineState(state) with
                override me.OnEvent(event:GumballEvent) =
                    match event with
                    | :? RefillGumballsEvent ->
                        state.Hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Start))
                        getReadyForQuarterState me
                    | :? InsertQuarterEvent ->
                        state.Hardware.OnNext(Input.ReturnQuarterEvent())
                        state.Hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Quarter))
                        me :> IGumballMachineState
                    | :? EjectQuarterEvent ->
                        state.Hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Eject))
                        me :> IGumballMachineState
                    | :? TurnCrankEvent ->
                        state.Hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Crank))
                        me :> IGumballMachineState
                    | :? TakeGumballEvent ->
                        state.Hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Take))
                        me :> IGumballMachineState
                    | _ -> 
                        base.OnEvent(event)
        }
        :> IGumballMachineState

    let create (hardware:IGumballHardware) =
        
        // create a default gumball machine state
        let state = ref (getSoldOutState (DefaultGumballMachineState(hardware)))

        // need a reference value, b/c of recursive dependencies*
        // 'onStateChanged' depends on 'machine'
        // 'subscription' depends on 'onStateChanged'
        // 'machine' depends on 'subscription'*
        let machine = ref Unchecked.defaultof<IGumballMachine>

        // listen to hardware events
        // mutate the state whenever on known hardware event
        let subscription =
            hardware.Subscribe(fun e -> state := state.Value.OnEvent(e))
        
        machine :=
            { new IGumballMachine with
                override me.Hardware = hardware
                override me.Dispose() = subscription.Dispose() }
        
        hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Start))

        machine.Value