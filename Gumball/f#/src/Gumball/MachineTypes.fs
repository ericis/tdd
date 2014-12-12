namespace Archient.DesignPatterns.Gumball
    
open System
open System.ComponentModel

open Archient.DesignPatterns.Gumball.Hardware

type IGumballMachine =
    inherit IDisposable
    inherit INotifyPropertyChanged

    abstract member Hardware : IGumballHardware with get