namespace Archient.DesignPatterns.Gumball

open TechTalk.SpecFlow

[<Binding>]
module FeatureSteps =

    open TechTalk.SpecFlow

    open Archient.DesignPatterns.Gumball
    open Archient.DesignPatterns.Gumball.Hardware.Events
    open Archient.Testing.Xunit

    let machine = ref Unchecked.defaultof<IGumballMachine>
    let receivedReturnQuarterEventAfterInsert = ref false

    let [<Given>] ``I have a new gumball machine`` () =

        machine := 
            GumballTests.createMachine()
            |> GumballTests.listenFor<Input.ReturnQuarterEvent> receivedReturnQuarterEventAfterInsert

    let [<When>] ``I refill it``() =
        
        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.RefillGumballsEvent()|]

    let [<When>] ``I insert a quarter``() =
        
        receivedReturnQuarterEventAfterInsert := false

        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.InsertQuarterEvent()|]

    let [<When>] ``I eject a quarter``() =
        
        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.EjectQuarterEvent()|]

    let [<When>] ``I turn the crank``() =
        
        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.TurnCrankEvent()|]

    let [<When>] ``a gumball is dispensed``() =
        
        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.GumballDispensedEvent()|]

    let [<When>] ``a gumball is not dispensed``() =
        
        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.OutOfGumballsEvent()|]

    let [<When>] ``I try to take a gumball``() =
        
        machine :=
            machine.Value
            |> GumballTests.raiseEvents [|Output.TakeGumballEvent()|]

    let [<Then>] ``it is not empty`` () =

        machine.Value.State.IsEmpty
        |> assertFalse

    let [<Then>] ``it is empty`` () =

        machine.Value.State.IsEmpty
        |> assertTrue

    let [<Then>] ``it has a quarter`` () =

        machine.Value.State.HasQuarter
        |> assertTrue

    let [<Then>] ``it has no quarter`` () =

        machine.Value.State.HasQuarter
        |> assertFalse

    let [<Then>] ``the display reads "(.*)"`` (message:string) =

        machine.Value.State.Message
        |> assertEqual message

    let [<Then>] ``it returns my quarter``() =

        receivedReturnQuarterEventAfterInsert.Value
        |> assertTrue