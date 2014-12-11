namespace Archient.DesignPatterns.Gumball

module GumballMachine =
    
    open System
    open System.ComponentModel
    open System.Diagnostics

    open Archient.DesignPatterns.Gumball.Hardware
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let private onRefill (state:GumballMachineState) =
        
        {
            IsEmpty = false
            HasQuarter = false
            IsDispensing = false
            Message = Messages.Ready.Start }

    let private onInsertQuarter (state:GumballMachineState) =
        
        Trace.TraceInformation(sprintf "onInsertQuarter: %A" state)

        let message = 
            match state.IsEmpty with
            | true -> Messages.SoldOut.Quarter
            | false -> 
                match state.HasQuarter with
                | true -> Messages.Quarter.Quarter
                | false -> 
                    match state.IsDispensing with
                    | true -> Messages.Crank.Quarter
                    | false -> Messages.Ready.Quarter

        {
            IsEmpty = state.IsEmpty
            HasQuarter = not state.IsEmpty && not state.IsDispensing
            IsDispensing = state.IsDispensing
            Message = message }

    let private onEjectQuarter (state:GumballMachineState) =
        
        Trace.TraceInformation(sprintf "onEjectQuarter: %A" state)
        
        let message = 
            match state.IsEmpty with
            | true -> Messages.SoldOut.Eject
            | false -> 
                match state.HasQuarter with
                | true -> Messages.Quarter.Eject
                | false -> 
                    match state.IsDispensing with
                    | true -> Messages.Crank.Eject
                    | false -> Messages.Ready.Eject

        {
            IsEmpty = state.IsEmpty
            HasQuarter = false
            IsDispensing = false
            Message = message }

    let private onTurnCrank (state:GumballMachineState) =
        
        Trace.TraceInformation(sprintf "onTurnCrank: %A" state)
        
        let message = 
            match state.IsEmpty with
            | true -> Messages.SoldOut.Crank
            | false -> 
                match state.HasQuarter with
                | true -> Messages.SoldOut.Quarter
                | false -> 
                    match state.IsDispensing with
                    | true -> Messages.Crank.Crank
                    | false -> Messages.Ready.Crank

        {
            IsEmpty = state.IsEmpty
            HasQuarter = state.HasQuarter
            IsDispensing = false
            Message = message }

    let private onGumballDispensed (state:GumballMachineState) =
        
        Trace.TraceInformation(sprintf "onGumballDispensed: %A" state)
        
        let message = Messages.Quarter.Crank
        
        {
            IsEmpty = false
            HasQuarter = false
            IsDispensing = true
            Message = message }

    let private onOutOfGumballs (state:GumballMachineState) =
        
        Trace.TraceInformation(sprintf "onOutOfGumballs: %A" state)
        
        let message = Messages.SoldOut.Quarter
        
        {
            IsEmpty = true
            HasQuarter = false
            IsDispensing = false
            Message = message }

    let private onTakeGumball (state:GumballMachineState) =
        
        Trace.TraceInformation(sprintf "onTakeGumball: %A" state)
        
        let message = 
            match state.IsEmpty with
            | true -> Messages.SoldOut.Take
            | false -> 
                match state.HasQuarter with
                | true -> Messages.Quarter.Take
                | false -> 
                    match state.IsDispensing with
                    | true -> Messages.Ready.Start
                    | false -> Messages.Ready.Take

        {
            IsEmpty = state.IsEmpty
            HasQuarter = state.HasQuarter
            IsDispensing = false
            Message = message }

    let private onHardwareEvent (e:GumballEvent) (state:GumballMachineState ref) (onStateChanged:unit->unit) (hardware:IGumballHardware) =
        
        Trace.TraceInformation(sprintf "onHardwareEvent: %A" (e.GetType().Name))
        
        match e with

        | :? RefillGumballsEvent -> 
            state := onRefill !state
            onStateChanged()

        | :? InsertQuarterEvent ->  
            
            // return quarter if
            //   - machine is empty
            //   - already has a quarter
            //   - machine is dispensing gumball
            if state.Value.IsEmpty || state.Value.HasQuarter || state.Value.IsDispensing then
                hardware.OnNext(Input.ReturnQuarterEvent())

            state := onInsertQuarter !state
            onStateChanged()

        | :? EjectQuarterEvent -> 
            
            // return quarter
            hardware.OnNext(Input.ReturnQuarterEvent())

            state := onEjectQuarter !state
            onStateChanged()

        | :? TurnCrankEvent -> 
            state := onTurnCrank !state
            onStateChanged()

        | :? GumballDispensedEvent -> 
            state := onGumballDispensed !state
            onStateChanged()

        | :? OutOfGumballsEvent -> 
            
            if state.Value.HasQuarter then
                hardware.OnNext(Input.ReturnQuarterEvent())

            state := onOutOfGumballs !state
            onStateChanged()

        | :? TakeGumballEvent -> 
            state := onTakeGumball !state
            onStateChanged()

        | _ -> 
            () // unrecognized event

    let create (hardware:IGumballHardware) =

        // create a default gumball machine state
        let state = 
            ref {
                IsEmpty = true
                HasQuarter = false
                IsDispensing = false
                Message = Messages.SoldOut.Start }
        
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
                override me.State = !state
                override me.Dispose() = subscription.Dispose() }

        machine.Value