namespace Archient.DesignPatterns.Gumball

module StateChangeTests =
    
    open System
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events

    let private testMachineStateNotification<'t when 't :> GumballEvent> (gumballEvent:'t) =
        let receivedStateChanged = ref false

        GumballTests.createMachine()
        |> GumballTests.listenForPropertyChanged "State" receivedStateChanged
        |> assertIsFalse(fun _ -> receivedStateChanged.Value)
        |> GumballTests.raiseEvents [|gumballEvent|]
        |> assertIsTrue(fun _ -> receivedStateChanged.Value)
        |> dispose
        
    let [<Fact>] ``machine state: refill, notifies state change``() = 
        
        testMachineStateNotification (Output.RefillGumballsEvent())

    let [<Fact>] ``machine state: insert quarter, notifies state change``() = 
        
        testMachineStateNotification (Output.InsertQuarterEvent())

    let [<Fact>] ``machine state: eject quarter, notifies state change``() = 
        
        testMachineStateNotification (Output.EjectQuarterEvent())

    let [<Fact>] ``machine state: turn crank, notifies state change``() = 
        
        testMachineStateNotification (Output.TurnCrankEvent())

    let [<Fact>] ``machine state: gumball dispensed, notifies state change``() = 
        
        testMachineStateNotification (Output.EjectQuarterEvent())

    let [<Fact>] ``machine state: out of gumballs, notifies state change``() = 
        
        testMachineStateNotification (Output.OutOfGumballsEvent())

    let [<Fact>] ``machine state: take gumball, notifies state change``() = 
        
        testMachineStateNotification (Output.TakeGumballEvent())