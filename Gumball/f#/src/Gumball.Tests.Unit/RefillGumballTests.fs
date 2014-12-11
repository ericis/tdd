namespace Archient.DesignPatterns.Gumball

module RefillGumballTests =
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let [<Fact>] ``refill: has gumballs``() = 
        
        GumballTests.createFullMachine()
        |> assertIsFalse (fun x -> x.State.IsEmpty)
        |> dispose

    let [<Fact>] ``refill: displays no quarter``() = 
        
        GumballTests.createFullMachine()
        |> assertAreEqual Messages.Ready.Start (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``refill: insert quarter, has quarter``() = 
        
        GumballTests.createFullMachineAndInsertQuarter()
        |> assertIsTrue (fun x -> x.State.HasQuarter)
        |> dispose

    let [<Fact>] ``refill: insert quarter, displays turn crank``() = 
        
        GumballTests.createFullMachineAndInsertQuarter()
        |> assertAreEqual Messages.Ready.Quarter (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``refill: eject quarter, displays no quarter``() = 
        
        GumballTests.createFullMachineAndEjectQuarter()
        |> assertAreEqual Messages.Ready.Eject (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``refill: turn crank, displays please pay``() = 
        
        GumballTests.createFullMachineAndTurnCrank()
        |> assertAreEqual Messages.Ready.Crank (fun x -> x.State.Message)
        |> dispose

    let [<Fact>] ``refill: take gumball displays need a quarter``() = 
        
        GumballTests.createFullMachineAndTakeGumball()
        |> assertAreEqual Messages.Ready.Take (fun x -> x.State.Message)
        |> dispose