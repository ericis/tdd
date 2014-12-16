namespace Archient.DesignPatterns.Gumball.Client
{
    using System;
    using Archient.DesignPatterns.Gumball.Domain;
    using Archient.DesignPatterns.Gumball.Hardware;

    /// <summary>
    /// Factory for creating gumball machines.
    /// </summary>
    public static class GumballMachineFactory
    {
        public static IGumballMachine Create(IGumballHardware hardware)
        {
            if (hardware == null) throw new ArgumentNullException("hardware");

            var machine = new GumballMachine(hardware);

            machine.Initialize();

            return machine;
        }
    }
}