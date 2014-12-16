namespace Archient.DesignPatterns.Gumball.Client
{
    using System;
    using Archient.DesignPatterns.Gumball.Hardware;

    /// <summary>
    /// Contract for a gumball machine
    /// </summary>
    public interface IGumballMachine : IDisposable
    {
        /// <summary>
        /// Gets the hardware.
        /// </summary>
        /// <value>
        /// The hardware.
        /// </value>
        IGumballHardware Hardware { get; }
    }
}