namespace Archient.DesignPatterns.Gumball

module QuarterGumballTests =
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let [<Fact>] ``quarter: insert 2nd quarter, returns quarter``() = 
        
        let receivedReturnQuarterEvent = ref false
        
        // Note: event raised is different than event listened for
        GumballTests.createFullMachineWithQuarter()
        |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEvent
        |> assertIsFalse (fun x -> receivedReturnQuarterEvent.Value)
        |> GumballTests.raiseEvents [|Output.InsertQuarterEvent()|]
        |> assertIsTrue (fun x -> receivedReturnQuarterEvent.Value)
        |> dispose

    let [<Fact>] ``quarter: insert 2nd quarter, has quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndInsertQuarter()
        |> assertIsTrue (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``quarter: insert 2nd quarter, displays already has quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndInsertQuarter()
        |> assertAreEqual Messages.Quarter.Quarter (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``quarter: eject quarter, has no quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndEjectQuarter()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``quarter: eject quarter, displays pick-up quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndEjectQuarter()
        |> assertAreEqual Messages.Quarter.Eject (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``quarter: turn crank, has quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndTurnCrank()
        |> assertIsTrue (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``quarter: turn crank w/o gumball, displays sold out w/ quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndTurnCrank()
        |> assertAreEqual Messages.SoldOut.Quarter (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``quarter: take gumball, displays turn crank``() = 
        
        GumballTests.createFullMachineWithQuarterAndTakeGumball()
        |> assertAreEqual Messages.Quarter.Take (fun x -> x.State.Message)
        |> dispose