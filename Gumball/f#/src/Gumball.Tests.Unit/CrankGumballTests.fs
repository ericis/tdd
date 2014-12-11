namespace Archient.DesignPatterns.Gumball

module CrankGumballTests =
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let [<Fact>] ``crank w/o gumball: is not dispensing``() = 
        
        GumballTests.createFullMachineWithQuarterAndTurnCrank()
        |> assertIsFalse (fun x -> x.State.IsDispensing)
        |> dispose

    let [<Fact>] ``crank w/o gumball: returns quarter``() = 
        
        let receivedReturnQuarterEvent = ref false
        
        // Note: event raised is different than event listened for
        GumballTests.createFullMachineWithQuarterAndTurnCrank()
        |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEvent
        |> assertIsFalse (fun x -> receivedReturnQuarterEvent.Value)
        |> GumballTests.raiseEvents [|Output.OutOfGumballsEvent()|]
        |> assertIsTrue (fun x -> receivedReturnQuarterEvent.Value)
        |> dispose

    let [<Fact>] ``crank w/o gumball: has no quarter``() = 
        
        GumballTests.createFullMachineWithQuarterCrankAndOutOfGumballs()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``crank w/ gumball: displays gumball is coming``() = 
        
        GumballTests.createFullMachineWithQuarterCrankAndGumballDispensed()
        |> assertAreEqual Messages.Quarter.Crank (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``crank w/ gumball: is dispensing``() = 
        
        GumballTests.createFullMachineWithQuarterCrankAndGumballDispensed()
        |> assertIsTrue (fun x -> x.State.IsDispensing)
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: insert 2nd quarter, returns quarter``() = 
        
        let receivedReturnQuarterEvent = ref false
        
        // Note: event raised is different than event listened for
        GumballTests.createFullMachineWithQuarterCrankAndGumballDispensed()
        |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEvent
        |> assertIsFalse (fun x -> receivedReturnQuarterEvent.Value)
        |> GumballTests.raiseEvents [|Output.InsertQuarterEvent()|]
        |> assertIsTrue (fun x -> receivedReturnQuarterEvent.Value)
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: insert 2nd quarter, displays please wait``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndInsertQuarter()
        |> assertAreEqual Messages.Crank.Quarter (fun x -> x.State.Message)
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: insert 2nd quarter, has no quarter``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndInsertQuarter()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: eject quarter, displays too late``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndEjectQuarter()
        |> assertAreEqual Messages.Crank.Eject (fun x -> x.State.Message)
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: turn crank 2nd time, displays no``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndTurnAgain()
        |> assertAreEqual Messages.Crank.Crank (fun x -> x.State.Message)
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: take gumball, displays no quarter``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndTakeGumball()
        |> assertAreEqual Messages.Ready.Start (fun x -> x.State.Message)
        |> dispose