[&lArr; TDD](../../)

# F# Test-Driven Development

[F# is a functional language](http://fsharp.org) that borrows heavily from traditional, 'purist' functional languages while balancing the necessary Object-Oriented Programming (OOP) paradigm of its .NET framework legacy.  As a result, F# becomes an incredible blend of tooling that supports both Functional and OOP choices as they are needed.

The F# solution provides examples of both unit tests and BDD tests supported by the same test library (DRY principle).

## Source Implementation

 * [Hardware Event Specification](../../f%23/src/Gumball.Hardware/EventTypes.fsi)
 * [Hardware Specification](../../f%23/src/Gumball.Hardware/HardwareTypes.fsi)
 * [Software Implementation](../../f%23/src/Gumball/GumballMachine.fs)
   (*start with the 'create' function at the bottom)

## Tests

### BDD Tests

 * [Sold Out Features](../../f%23/src/Gumball.Tests.Features/SoldOut.feature)
 * [Awaiting Quarter Features](../../f%23/src/Gumball.Tests.Features/Refill.feature)
 * [Has Quarter Features](../../f%23/src/Gumball.Tests.Features/Quarter.feature)
 * [Crank is Turned Features](../../f%23/src/Gumball.Tests.Features/Crank.feature)

### Unit Tests

 * [Sold Out](../../f%23/src/Gumball.Tests.Unit/SoldOutGumballTests.fs)
 * [Awaiting Quarter](../../f%23/src/Gumball.Tests.Unit/RefillGumballTests.fs)
 * [Has Quarter](../../f%23/src/Gumball.Tests.Unit/QuarterGumballTests.fs)
 * [Crank is Turned](../../f%23/src/Gumball.Tests.Unit/CrankGumballTests.fs)

## Design Decisions

 * .NET v4.5
 * Minimize public API
 * Maximize use of functions
 * Minimize mutation of state
 * Implement state machine as simple functional state transitions associated with hardware events. Mutate the gumball machine's state each type a state transition is made.
 * Ensure hardware dependency can be swapped with a test-double (stub/mock/fake)
 * Event-Driven Architecture (EDA): Publish/Subscribe
   .NET generics and the implementation of IObservable and IObserver make abstracting the hardware device as a set of input and output events simple to implement and to understand.
 * Wrap [xUnit](http://github.com/xunit/xunit) assertions as F# functions to streamline a more functional readability of tests
 * Use [object expressions](http://msdn.microsoft.com/en-us/library/dd233237.aspx) to avoid the extra code and overhead that is required to create a new, named type. If you use object expressions to minimize the number of types created in a program, you can reduce the number of lines of code and prevent the unnecessary proliferation of types. Instead of creating many types just to handle specific situations, you can use an object expression that customizes an existing type or provides an appropriate implementation of an interface for the specific case at hand.

## Solution Pros

 * It's F#! (personal preference) :)
 * Functional design strives to minimize state mutations, which reduces surface area for state transitions and any analysis to discover these transitions, and, according to studies, should also result in the reduction of bugs in the system.  Development must still adhere to this principle, as F# doesn't (can't) strictly enforce this.
 * Internal state management composed using function closures without requiring state management through named type definitions (e.g. classes).
 * Support for ``` ``readable method names``() ``` using double backtick quotes (``).  Use is limited to create readable test method names and is never used in application code.  This eliminated the duplication of SpecFlow test names (C# requires an attribute with the readable name and a method name).
 * Support for [object expressions](http://msdn.microsoft.com/en-us/library/dd233237.aspx) that enable inline interface implementations, without the need to define an explicit class implementation.  This helps to maintain the functional API surface while still supporting the ability to group functions as a single dependency in an interface definition that can be passed between functions as a single value.
 * EDA kept the implementation and API clean

## Solution Cons

 * TBD: After other languages are implemented

## Dependencies
 
 * Obvious: .NET and Visual Studio
 * [xUnit](http://github.com/xunit/xunit)
   Authors of the popular "xUnit Test Patterns", the xUnit library provides a solid platform for test automation.
 * [SpecFlow](http://www.specflow.org)
   A Visual Studio extension supports writing BDD tests as readable "Feature" files supported by the Gherkin (Given-When-Then) syntax. SpecFlow feature files must be implemented in a C# project, but test steps are still defined in F# to keep as much language consistency as possible.
