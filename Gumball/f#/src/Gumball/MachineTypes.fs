namespace Archient.DesignPatterns.Gumball
    
open System
open System.ComponentModel

open Archient.DesignPatterns.Gumball.Hardware

type IGumballMachine =
    inherit IDisposable

    abstract member Hardware : IGumballHardware with get