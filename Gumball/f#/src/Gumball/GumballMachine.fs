namespace Archient.DesignPatterns.Gumball

module GumballMachine =
    
    open System
    open System.ComponentModel
    open System.Diagnostics

    open Archient.DesignPatterns.Gumball.Hardware
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output
    
    type private GumballMachineState = 
        {
            IsEmpty: bool
            HasQuarter: bool
            IsDispensing: bool }

    let private onRefill (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onRefill: %A" state)
        
        hardware.OnNext(Input.DisplayMessageEvent(Messages.Ready.Start))

        {
            IsEmpty = false
            HasQuarter = state.Value.HasQuarter
            IsDispensing = false }

    let private onInsertQuarter (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onInsertQuarter: %A" state)

        let message = 
            match state.Value.IsEmpty with
            | true -> Messages.SoldOut.Quarter
            | false -> 
                match state.Value.HasQuarter with
                | true -> Messages.Quarter.Quarter
                | false -> 
                    match state.Value.IsDispensing with
                    | true -> Messages.Crank.Quarter
                    | false -> Messages.Ready.Quarter
        
        // return quarter if
        //   - machine is empty
        //   - already has a quarter
        //   - machine is dispensing gumball
        if state.Value.IsEmpty || state.Value.HasQuarter || state.Value.IsDispensing then
            hardware.OnNext(Input.ReturnQuarterEvent())
                    
        hardware.OnNext(Input.DisplayMessageEvent(message))
        
        {
            IsEmpty = state.Value.IsEmpty
            HasQuarter = not state.Value.IsEmpty && not state.Value.IsDispensing
            IsDispensing = state.Value.IsDispensing }

    let private onEjectQuarter (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onEjectQuarter: %A" state)

        let message = 
            match state.Value.IsEmpty with
            | true -> Messages.SoldOut.Eject
            | false -> 
                match state.Value.HasQuarter with
                | true -> Messages.Quarter.Eject
                | false -> 
                    match state.Value.IsDispensing with
                    | true -> Messages.Crank.Eject
                    | false -> Messages.Ready.Eject
            
        // return quarter
        hardware.OnNext(Input.ReturnQuarterEvent())

        hardware.OnNext(Input.DisplayMessageEvent(message))

        {
            IsEmpty = state.Value.IsEmpty
            HasQuarter = false
            IsDispensing = false }

    let private onTurnCrank (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onTurnCrank: %A" state)
        
        let message = 
            match state.Value.IsEmpty with
            | true -> Messages.SoldOut.Crank
            | false -> 
                match state.Value.HasQuarter with
                | true -> Messages.SoldOut.Quarter
                | false -> 
                    match state.Value.IsDispensing with
                    | true -> Messages.Crank.Crank
                    | false -> Messages.Ready.Crank

        hardware.OnNext(Input.DisplayMessageEvent(message))
        
        {
            IsEmpty = state.Value.IsEmpty
            HasQuarter = state.Value.HasQuarter
            IsDispensing = false }

    let private onGumballDispensed (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onGumballDispensed: %A" state)
        
        let message = Messages.Quarter.Crank

        hardware.OnNext(Input.DisplayMessageEvent(message))
        
        {
            IsEmpty = false
            HasQuarter = false
            IsDispensing = true }

    let private onOutOfGumballs (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onOutOfGumballs: %A" state)
        
        if state.Value.HasQuarter then
            hardware.OnNext(Input.ReturnQuarterEvent())

        let message = Messages.SoldOut.Quarter

        hardware.OnNext(Input.DisplayMessageEvent(message))
        
        {
            IsEmpty = true
            HasQuarter = false
            IsDispensing = false }

    let private onTakeGumball (hardware:IGumballHardware) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onTakeGumball: %A" state)
        
        let message = 
            match state.Value.IsEmpty with
            | true -> Messages.SoldOut.Take
            | false -> 
                match state.Value.HasQuarter with
                | true -> Messages.Quarter.Take
                | false -> 
                    match state.Value.IsDispensing with
                    | true -> Messages.Ready.Start
                    | false -> Messages.Ready.Take

        hardware.OnNext(Input.DisplayMessageEvent(message))
        
        {
            IsEmpty = state.Value.IsEmpty
            HasQuarter = state.Value.HasQuarter
            IsDispensing = false }

    let private onDisplay (message:string) (state:GumballMachineState ref) =
        
        Trace.TraceInformation(sprintf "onDisplay: %s" message)
        
        {
            IsEmpty = state.Value.IsEmpty
            HasQuarter = state.Value.HasQuarter
            IsDispensing = state.Value.IsDispensing }

    let private onHardwareEvent (e:GumballEvent) (state:GumballMachineState ref) (onStateChanged:unit->unit) (hardware:IGumballHardware) =
        
        Trace.TraceInformation(sprintf "onHardwareEvent: %A" (e.GetType().Name))
        
        match e with

        | :? Input.DisplayMessageEvent as displayMessage ->
            state := onDisplay displayMessage.Value state

        | :? RefillGumballsEvent -> 
            state := onRefill hardware state

        | :? InsertQuarterEvent ->  
            state := onInsertQuarter hardware state

        | :? EjectQuarterEvent -> 
            state := onEjectQuarter hardware state

        | :? TurnCrankEvent -> 
            state := onTurnCrank hardware state

        | :? GumballDispensedEvent -> 
            state := onGumballDispensed hardware state

        | :? OutOfGumballsEvent -> 
            state := onOutOfGumballs hardware state

        | :? TakeGumballEvent -> 
            state := onTakeGumball hardware state
        | _ -> 
            () // unrecognized event
        
        onStateChanged()

    let create (hardware:IGumballHardware) =

        // create a default gumball machine state
        let state = 
            ref {
                IsEmpty = true
                HasQuarter = false
                IsDispensing = false }

        // need a reference value, b/c of recursive dependencies*
        // 'onStateChanged' depends on 'machine'
        // 'subscription' depends on 'onStateChanged'
        // 'machine' depends on 'subscription'*
        let machine = ref Unchecked.defaultof<IGumballMachine>
        
        let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

        // notify clients that the state changed (in case they are intersted)
        let onStateChanged () =
            propertyChanged.Trigger(machine.Value, new PropertyChangedEventArgs("State"))

        // listen to hardware events
        // mutate the state whenever on known hardware event
        let subscription =
            hardware.Subscribe(fun e -> onHardwareEvent e state onStateChanged hardware)
        
        machine :=
            { new IGumballMachine with
                override me.add_PropertyChanged(handler) = propertyChanged.Publish.AddHandler(handler)
                override me.remove_PropertyChanged(handler) = propertyChanged.Publish.RemoveHandler(handler)
                override me.Hardware = hardware
                override me.Dispose() = subscription.Dispose() }
        
        hardware.OnNext(Input.DisplayMessageEvent(Messages.SoldOut.Start))

        machine.Value