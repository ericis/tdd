namespace Archient.DesignPatterns.Gumball
    
open System
open System.ComponentModel

open Archient.DesignPatterns.Gumball.Hardware

type GumballMachineState = 
    {
        IsEmpty: bool
        HasQuarter: bool
        IsDispensing: bool }

type IGumballMachine =
    inherit IDisposable
    inherit INotifyPropertyChanged

    abstract member Hardware : IGumballHardware with get
    abstract member State : GumballMachineState with get