namespace Archient.DesignPatterns.Gumball

module RefillGumballTests =
    
    open Xunit
    open Archient.Testing.Xunit
    
    open Archient.DesignPatterns.Gumball.Hardware.Events.Output

    let [<Fact>] ``refill: displays no quarter``() = 
        
        GumballTests.createFullMachine()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Ready.Start
        |> dispose

    let [<Fact>] ``refill: insert quarter, displays turn crank``() = 
        
        GumballTests.createFullMachineAndInsertQuarter()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Ready.Quarter
        |> dispose

    let [<Fact>] ``refill: eject quarter, displays no quarter``() = 
        
        GumballTests.createFullMachineAndEjectQuarter()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Ready.Eject
        |> dispose

    let [<Fact>] ``refill: turn crank, displays please pay``() = 
        
        GumballTests.createFullMachineAndTurnCrank()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Ready.Crank
        |> dispose

    let [<Fact>] ``refill: take gumball displays need a quarter``() = 
        
        GumballTests.createFullMachineAndTakeGumball()
        |> GumballTests.assertLastDisplayMessageEquals Messages.Ready.Take
        |> dispose