namespace Archient.DesignPatterns.Gumball

module GumballMachine =
    
    open System
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
            HasQuarter = false
            IsDispensing = false
            Message = message }

    let private onHardwareEvent (e:GumballEvent) (state:GumballMachineState ref) (hardware:IGumballHardware) =
        
        Trace.TraceInformation(sprintf "onHardwareEvent: %A" (e.GetType().Name))
        
        match e with

        | :? RefillGumballsEvent -> 
            state := onRefill !state

        | :? InsertQuarterEvent ->  
            
            // return quarter if
            //   - machine is empty
            //   - already has a quarter
            //   - machine is dispensing gumball
            if state.Value.IsEmpty || state.Value.HasQuarter || state.Value.IsDispensing then
                hardware.OnNext(Input.ReturnQuarterEvent())

            state := onInsertQuarter !state

        | :? EjectQuarterEvent -> 
            
            // return quarter
            hardware.OnNext(Input.ReturnQuarterEvent())

            state := onEjectQuarter !state

        | :? TurnCrankEvent -> 
            state := onTurnCrank !state

        | :? GumballDispensedEvent -> 
            state := onGumballDispensed !state

        | :? OutOfGumballsEvent -> 
            
            if state.Value.HasQuarter then
                hardware.OnNext(Input.ReturnQuarterEvent())

            state := onOutOfGumballs !state

        | :? TakeGumballEvent -> 
            state := onTakeGumball !state

        | _ -> 
            ()

    let create (hardware:IGumballHardware) =

        // create a default gumball machine state
        let state = 
            ref {
                IsEmpty = true
                HasQuarter = false
                IsDispensing = false
                Message = Messages.SoldOut.Start }
        
        // listen to hardware events
        // mutate the state whenever on known hardware event
        let subscription =
            hardware.Subscribe(fun e -> onHardwareEvent e state hardware)
        
        { new IGumballMachine with
            override me.Hardware = hardware
            override me.State = !state
            override me.Dispose() = subscription.Dispose() }