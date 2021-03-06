﻿namespace Archient.DesignPatterns.Gumball

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

    let [<Fact>] ``quarter: insert 2nd quarter, displays already has quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndInsertQuarter()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Quarter.Quarter
        |> dispose

    let [<Fact>] ``quarter: eject quarter, displays pick-up quarter``() = 
        
        GumballTests.createFullMachineWithQuarterAndEjectQuarter()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Quarter.Eject
        |> dispose

    let [<Fact>] ``quarter: turn crank w/o gumball, displays sold out w/ quarter``() = 
        
        GumballTests.createFullMachineWithQuarterCrankAndOutOfGumballs()
        |> GumballTests.assertLastDisplayMessageEquals Messages.SoldOut.Quarter
        |> dispose

    let [<Fact>] ``quarter: take gumball, displays turn crank``() = 
        
        GumballTests.createFullMachineWithQuarterAndTakeGumball()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Quarter.Take
        |> dispose