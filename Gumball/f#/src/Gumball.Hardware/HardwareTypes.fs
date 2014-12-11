namespace Archient.DesignPatterns.Gumball.Hardware

open System

open Archient.DesignPatterns.Gumball.Hardware.Events

type IGumballHardware =
    inherit IObserver<GumballEvent>
    inherit IObservable<GumballEvent>