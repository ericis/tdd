namespace Archient.DesignPatterns.Gumball.Hardware.Events
{
    /// <summary>Generic base class raised whenever a gumball hardware event occurs.</summary>
    /// <typeparam name="T">The type of argument passed in the event</typeparam>
    public abstract class GumballEvent<T> : GumballEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GumballEvent{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public GumballEvent(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the argument value passed in the event
        /// </summary>
        /// <value>
        /// The argument value passed in the event
        /// </value>
        public T Value { get; private set; }
    }
}