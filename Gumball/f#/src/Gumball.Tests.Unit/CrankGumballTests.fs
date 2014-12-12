namespace Archient.DesignPatterns.Gumball

module CrankGumballTests =
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let [<Fact>] ``crank w/o gumball: returns quarter``() = 
        
        let receivedReturnQuarterEvent = ref false
        
        // Note: event raised is different than event listened for
        GumballTests.createFullMachineWithQuarterAndTurnCrank()
        |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEvent
        |> assertIsFalse (fun x -> receivedReturnQuarterEvent.Value)
        |> GumballTests.raiseEvents [|Output.OutOfGumballsEvent()|]
        |> assertIsTrue (fun x -> receivedReturnQuarterEvent.Value)
        |> dispose

    let [<Fact>] ``crank w/ gumball: displays gumball is coming``() = 
        
        GumballTests.createFullMachineWithQuarterCrankAndGumballDispensed()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Quarter.Crank
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
        |> GumballTests.assertLastDisplayMessageEquals Messages.Crank.Quarter
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: eject quarter, displays too late``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndEjectQuarter()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Crank.Eject
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: turn crank 2nd time, displays no``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndTurnAgain()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Crank.Crank
        |> dispose
    
    let [<Fact>] ``crank w/ gumball: take gumball, displays no quarter``() = 
        
        GumballTests.createFullMachineWithQuarterCrankGumballDispensedAndTakeGumball()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Ready.Start
        |> dispose