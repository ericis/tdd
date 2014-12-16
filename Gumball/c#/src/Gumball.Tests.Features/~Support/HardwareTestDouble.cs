namespace Archient.DesignPatterns.Gumball.Tests.Features
{
    using System;
    using System.Collections.Generic;
    using Archient.DesignPatterns.Gumball.Hardware;
    using Archient.DesignPatterns.Gumball.Hardware.Events;

    public class HardwareTestDouble : IGumballHardware
    {
        private readonly IList<IObserver<GumballEvent>> observers;

        public HardwareTestDouble()
        {
            this.observers = new List<IObserver<GumballEvent>>();
        }

        public IDisposable Subscribe(IObserver<GumballEvent> observer)
        {
            this.observers.Add(observer);
            
            return new DisposableObserver(this.observers, observer);
        }

        public void OnCompleted()
        {
            foreach (var observer in this.observers)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in this.observers)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(GumballEvent value)
        {
            foreach (var observer in this.observers)
            {
                observer.OnNext(value);
            }
        }

        private class DisposableObserver : IDisposable
        {
            private readonly IList<IObserver<GumballEvent>> observers;
            private readonly IObserver<GumballEvent> observer;

            public DisposableObserver(
                IList<IObserver<GumballEvent>> observers,
                IObserver<GumballEvent> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                this.observers.Remove(this.observer);
            }
        }
    }
}