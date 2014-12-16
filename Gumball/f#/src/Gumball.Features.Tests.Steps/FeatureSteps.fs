namespace Archient.DesignPatterns.Gumball

open TechTalk.SpecFlow

module TestState =
    
    open TechTalk.SpecFlow

    open Archient.DesignPatterns.Gumball
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.Testing.Xunit
    
    let private machineReference = ref Unchecked.defaultof<IGumballMachine>
    let private receivedReturnQuarterEventAfterInsert = ref false

    let createMachine () =
        
        // mutate stored machine reference
        machineReference :=
            // create the machine
            // listen for returned quarter and mutate state flag
            GumballTests.createMachine()
            |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEventAfterInsert

    let resetReceivedReturnQuarterEventListener () =
        receivedReturnQuarterEventAfterInsert := false

    let private assertMachineCreated () =
        assertNotNull(machineReference.Value)

    let assertReceivedReturnQuarterEvent () =
        // make sure the machine was created
        assertMachineCreated()

        // event listener assertion
        receivedReturnQuarterEventAfterInsert.Value
        |> assertTrue

    let getMachine () =
        // make sure the machine was created
        assertMachineCreated()

        // return machine reference
        machineReference.Value

    let raiseEvent<'t when 't :> GumballEvent and 't : (new:unit->'t)> () =
        // create event
        let gumballEvent = new 't()
        
        // raise event
        getMachine()
        |> GumballTests.raiseEvents [|gumballEvent|]
        |> ignore // end fluent API calls

[<Binding>]
module FeatureSteps =

    open TechTalk.SpecFlow

    open Archient.DesignPatterns.Gumball
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.Testing.Xunit

    let [<Given>] ``I have a new gumball machine`` () =
        
        ignore <| TestState.createMachine()

    let [<When>] ``I refill it``() =
        
        TestState.raiseEvent<Output.RefillGumballsEvent>()

    let [<When>] ``I insert a quarter``() =
        
        // reset state flag for event
        TestState.resetReceivedReturnQuarterEventListener()
        
        TestState.raiseEvent<Output.InsertQuarterEvent>()

    let [<When>] ``I eject a quarter``() =
        
        TestState.raiseEvent<Output.EjectQuarterEvent>()

    let [<When>] ``I turn the crank``() =
        
        TestState.raiseEvent<Output.TurnCrankEvent>()

    let [<When>] ``a gumball is dispensed``() =
        
        TestState.raiseEvent<Output.GumballDispensedEvent>()

    let [<When>] ``a gumball is not dispensed``() =
        
        TestState.raiseEvent<Output.OutOfGumballsEvent>()

    let [<When>] ``I try to take a gumball``() =
        
        TestState.raiseEvent<Output.TakeGumballEvent>()

    let [<Then>] ``the display reads "(.*)"`` (message:string) =
        
        // raise events
        TestState.getMachine()
        |> GumballTests.assertLastDisplayMessageEquals message
        |> ignore // end fluent API calls

    let [<Then>] ``it returns my quarter``() =
        
        TestState.assertReceivedReturnQuarterEvent()