namespace Archient.DesignPatterns.Gumball
    
open System

open Archient.DesignPatterns.Gumball.Hardware

type GumballMachineState = 
    {
        IsEmpty: bool
        HasQuarter: bool
        IsDispensing: bool
        Message: string }

type IGumballMachine =
    inherit IDisposable

    abstract member Hardware : IGumballHardware with get
    abstract member State : GumballMachineState with get