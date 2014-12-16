namespace Archient.DesignPatterns.Gumball
    
open System

open Archient.DesignPatterns.Gumball.Hardware

/// Contract for a gumball machine
type IGumballMachine =
    
    // internally handle subscriptions to hardware events that can be disposed of
    inherit IDisposable

    /// Gets the hardware this machine is coupled to
    abstract member Hardware : IGumballHardware with get