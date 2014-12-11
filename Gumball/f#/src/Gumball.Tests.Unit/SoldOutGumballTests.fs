namespace Archient.DesignPatterns.Gumball

module SoldOutGumballTests =
    
    open System
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let [<Fact>] ``new machine: is empty``() = 
        
        GumballTests.createMachine()
        |> assertIsTrue (fun x -> x.State.IsEmpty)
        |> dispose

    let [<Fact>] ``new machine: displays empty``() = 
        
        GumballTests.createMachine()
        |> assertAreEqual Messages.SoldOut.Start (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``new machine: has no quarter``() = 
        
        GumballTests.createMachine()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``new machine: insert quarter, returns quarter``() = 
        
        let receivedReturnQuarterEvent = ref false
        
        // Note: event raised is different than event listened for
        GumballTests.createMachine()
        |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEvent
        |> assertIsFalse (fun x -> receivedReturnQuarterEvent.Value)
        |> GumballTests.raiseEvents [|Output.InsertQuarterEvent()|]
        |> assertIsTrue (fun x -> receivedReturnQuarterEvent.Value)
        |> dispose

    let [<Fact>] ``new machine: insert quarter, has no quarter``() = 
        
        GumballTests.createMachineAndInsertQuarter()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``new machine: insert quarter, displays pickup quarter``() = 
        
        GumballTests.createMachineAndInsertQuarter()
        |> assertAreEqual Messages.SoldOut.Quarter (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``new machine: eject quarter, has no quarter``() = 
        
        GumballTests.createMachineAndEjectQuarter()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``new machine: eject quarter, displays no quarter``() = 
        
        GumballTests.createMachineAndEjectQuarter()
        |> assertAreEqual Messages.SoldOut.Eject (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``new machine: turn crank, displays no gumballs``() = 
        
        GumballTests.createMachineAndTurnCrank()
        |> assertAreEqual Messages.SoldOut.Crank (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``new machine: take gumball, displays no gumballs``() = 
        
        GumballTests.createMachineAndTakeGumball()
        |> assertAreEqual Messages.SoldOut.Take (fun x -> x.State.Message)
        |> dispose