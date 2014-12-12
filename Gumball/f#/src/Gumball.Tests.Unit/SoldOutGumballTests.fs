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
        |> GumballTests.assertLastDisplayMessageEquals Messages.SoldOut.Start
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
        |> GumballTests.assertLastDisplayMessageEquals Messages.SoldOut.Quarter
        |> dispose

    let [<Fact>] ``new machine: eject quarter, has no quarter``() = 
        
        GumballTests.createMachineAndEjectQuarter()
        |> assertIsFalse (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``new machine: eject quarter, displays no quarter``() = 
        
        GumballTests.createMachineAndEjectQuarter()
        |> GumballTests.assertLastDisplayMessageEquals Messages.SoldOut.Eject
        |> dispose

    let [<Fact>] ``new machine: turn crank, displays no gumballs``() = 
        
        GumballTests.createMachineAndTurnCrank()
        |> GumballTests.assertLastDisplayMessageEquals Messages.SoldOut.Crank
        |> dispose

    let [<Fact>] ``new machine: take gumball, displays no gumballs``() = 
        
        GumballTests.createMachineAndTakeGumball()
        |> GumballTests.assertLastDisplayMessageEquals Messages.SoldOut.Take
        |> dispose