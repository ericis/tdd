namespace Archient.DesignPatterns.Gumball
        
module GumballTests =

    open System
    open System.Collections.Generic

    open Archient.DesignPatterns.Gumball
    open Archient.DesignPatterns.Gumball.Hardware
    open Archient.DesignPatterns.Gumball.Hardware.Events

    let createTestHardware() =
        // state: list of subscribers
        let subscribers = 
            new List<IObserver<GumballEvent>>()
        
        // new hardware for tests
        // (closure of 'subscribers')
        { new IGumballHardware with
            
            override me.OnCompleted() = 
                // notify all: complete
                subscribers
                |> Seq.iter (fun subscriber -> subscriber.OnCompleted())
            
            override me.OnError(error) = 
                // notify all: error
                subscribers
                |> Seq.iter (fun subscriber -> subscriber.OnError(error))
            
            override me.OnNext(gumballEvent) = 
                // notify all: next event
                subscribers
                |> Seq.iter (fun subscriber -> subscriber.OnNext(gumballEvent))
            
            override me.Subscribe(subscriber) = 
                // add the subscriber
                subscribers.Add(subscriber)

                // return a subscription that can be disposed of
                // (closure of 'subscribers' and 'subscriber')
                {
                    new IDisposable with
                        override me.Dispose() = 
                            // remove subscriber on disposal
                            ignore <| subscribers.Remove(subscriber)
                } }

    let listenFor<'t when 't :> GumballEvent> (notified:bool ref) (machine:IGumballMachine) =
        
        // add listener
        // (closure of 'notified')
        machine.Hardware.Add(
            fun gumballEvent -> 
                
                // check if event type matches the type we're listening for
                match gumballEvent with
                | :? 't -> notified := true
                | _ -> ())

        machine

    let raiseEvents ([<ParamArray>]events:GumballEvent[]) (machine:IGumballMachine) =
        
        // raise events
        events
        |> Seq.iter (fun e -> machine.Hardware.OnNext(e))

        machine

    let createMachine () =
        
        createTestHardware()
        |> GumballMachine.create

    let createMachineAndRaiseEvents ([<ParamArray>]events:GumballEvent[]) =
        createTestHardware()
        |> GumballMachine.create
        |> raiseEvents events

    let createMachineAndInsertQuarter () =
        createMachineAndRaiseEvents [|Output.InsertQuarterEvent()|]

    let createMachineAndEjectQuarter () =
        createMachineAndRaiseEvents [|Output.EjectQuarterEvent()|]

    let createMachineAndTurnCrank () =
        createMachineAndRaiseEvents [|Output.TurnCrankEvent()|]

    let createMachineAndTakeGumball () =
        createMachineAndRaiseEvents [|Output.TakeGumballEvent()|]

    let createFullMachine () =
        createMachineAndRaiseEvents [|Output.RefillGumballsEvent()|]
    
    let createFullMachineAndInsertQuarter () =
        createFullMachine()
        |> raiseEvents [|Output.InsertQuarterEvent()|]
    
    let createFullMachineAndEjectQuarter () =
        createFullMachine()
        |> raiseEvents [|Output.EjectQuarterEvent()|]
    
    let createFullMachineAndTurnCrank () =
        createFullMachine()
        |> raiseEvents [|Output.TurnCrankEvent()|]
    
    let createFullMachineAndTakeGumball () =
        createFullMachine()
        |> raiseEvents [|Output.TakeGumballEvent()|]

    let createFullMachineWithQuarter () =
        createMachineAndRaiseEvents [|Output.RefillGumballsEvent();Output.InsertQuarterEvent()|]
    
    let createFullMachineWithQuarterAndInsertQuarter () =
        createFullMachineWithQuarter()
        |> raiseEvents [|Output.InsertQuarterEvent()|]
    
    let createFullMachineWithQuarterAndEjectQuarter () =
        createFullMachineWithQuarter()
        |> raiseEvents [|Output.EjectQuarterEvent()|]
    
    let createFullMachineWithQuarterAndTurnCrank () =
        createFullMachineWithQuarter()
        |> raiseEvents [|Output.TurnCrankEvent()|]
    
    let createFullMachineWithQuarterAndTakeGumball () =
        createFullMachineWithQuarter()
        |> raiseEvents [|Output.TakeGumballEvent()|]
    
    let createFullMachineWithQuarterCrankAndInsertQuarter () =
        createFullMachineWithQuarterAndTurnCrank()
        |> raiseEvents [|Output.InsertQuarterEvent()|]
    
    let createFullMachineWithQuarterCrankAndEjectQuarter () =
        createFullMachineWithQuarterAndTurnCrank()
        |> raiseEvents [|Output.EjectQuarterEvent()|]
    
    let createFullMachineWithQuarterCrankAndTurnAgain () =
        createFullMachineWithQuarterAndTurnCrank()
        |> raiseEvents [|Output.TurnCrankEvent()|]
    
    let createFullMachineWithQuarterCrankAndTakeGumball () =
        createFullMachineWithQuarterAndTurnCrank()
        |> raiseEvents [|Output.TakeGumballEvent()|]

    let createFullMachineWithQuarterCrankAndGumballDispensed () =
        createFullMachineWithQuarterAndTurnCrank()
        |> raiseEvents [|Output.GumballDispensedEvent()|]
    
    let createFullMachineWithQuarterCrankAndOutOfGumballs () =
        createFullMachineWithQuarterAndTurnCrank()
        |> raiseEvents [|Output.OutOfGumballsEvent()|]
    
    let createFullMachineWithQuarterCrankGumballDispensedAndInsertQuarter () =
        createFullMachineWithQuarterCrankAndGumballDispensed()
        |> raiseEvents [|Output.InsertQuarterEvent()|]
    
    let createFullMachineWithQuarterCrankGumballDispensedAndEjectQuarter () =
        createFullMachineWithQuarterCrankAndGumballDispensed()
        |> raiseEvents [|Output.EjectQuarterEvent()|]
    
    let createFullMachineWithQuarterCrankGumballDispensedAndTurnAgain () =
        createFullMachineWithQuarterCrankAndGumballDispensed()
        |> raiseEvents [|Output.TurnCrankEvent()|]
    
    let createFullMachineWithQuarterCrankGumballDispensedAndTakeGumball () =
        createFullMachineWithQuarterCrankAndGumballDispensed()
        |> raiseEvents [|Output.TakeGumballEvent()|]