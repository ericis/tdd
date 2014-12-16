namespace Archient.DesignPatterns.Gumball.Hardware.Events.Input
{
    /// <summary>Raised to request that the hardware display a message</summary>
    public class DisplayMessageEvent : GumballEvent<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessageEvent"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DisplayMessageEvent(string message)
            : base(message)
        {
        }
    }
}