[&lArr; TDD](../../)

# C# Test-Driven Development

TODO: In progress...

## Source Implementation

 * [Hardware Event Specification](./Gumball.Hardware/Events)
 * [Hardware Specification](./Gumball.Hardware/IGumballHardware.cs)
 * TODO: Software Implementation

## TODO: Tests

### BDD Tests

 * [Sold Out Features](./Gumball.Tests.Features/SoldOut.feature)
 * [Awaiting Quarter Features](./Gumball.Tests.Features/Refill.feature)
 * [Has Quarter Features](./Gumball.Tests.Features/Quarter.feature)
 * [Crank is Turned Features](./Gumball.Tests.Features/Crank.feature)

### Unit Tests

C# unit tests would be replicating the BDD feature tests and the [F# unit tests](../../f#/src/Gumball.Tests.Unit) already provide a .NET implementation reference (albeit unfamilar to most OOP developers).

## Design Decisions

 * .NET v4.5
 * Minimize public API
 * Leverage a client factory method to hide all implementation as internal
 * Implement state machine
 * Ensure hardware dependency can be swapped with a test-double (stub/mock/fake)
 * Event-Driven Architecture (EDA): Publish/Subscribe
   .NET generics and the implementation of IObservable and IObserver make abstracting the hardware device as a set of input and output events simple to implement and to understand.

## Solution Pros

 * It's C#! (for all those C# devs)
   Familiar to majority of developers (OOP language)
 * EDA kept the implementation and API clean

## Solution Cons

 * Proliferation of types as compared to [F# design decision to use object expressions](../../f#/src)
 * Lots of "language ceremony" to declare types and variables

## Dependencies
 
 * Obvious: .NET and Visual Studio
 * MSTest
   No particular reason other than that the [F# implementation](../../f#/src) used [xUnit](http://github.com/xunit/xunit).
 * [SpecFlow](http://www.specflow.org)
   A Visual Studio extension supports writing BDD tests as readable "Feature" files supported by the Gherkin (Given-When-Then) syntax. SpecFlow feature files must be implemented in a C# project, but test steps are still defined in F# to keep as much language consistency as possible.
