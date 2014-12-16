#Test-Driven Development

These 'Gumball' projects were created as a set of technology solutions focused on a single problem domain using [Test-Driven Development](http://en.wikipedia.org/wiki/Test-driven_development) (TDD).  The solutions used a test-first methodology rather than writing tests after.  In some cases, a [Behavior-Driven Development](http://en.wikipedia.org/wiki/Behavior-driven_development) (BDD) methodology is used.

## Gumball Machine

The team is tasked with creating a software solution for a gumball machine constrained to use a hardware API provided by an 'external' team and a set of requirements provided by the business' product owner (PO).  These requirements have been captured as User Stories.

User stories are written in the standard form:
    1. "As a [user]..."
    2. "I want [feature]..."
    3. "So that [reason]...".

User stories may alternatively appear as:
    1. "In order to [reason]..."
    2. "As a [user]..."
    3. "I want [feature]".

The IT analyst has worked with the product owner (business) to capture each story's set of Conditions of Acceptance (CoA) in [Gherkin syntax](https://github.com/cucumber/cucumber/wiki/Gherkin) (Given-When-Then) to support the IT team with test automation.

### Hardware

The hardware API is specified by an 'external' team according to the target technology stack.  As a result, hardware technology solutions range from a simple interface definition to more a complex event-driven architecture.

### Software

The team is tasked with translating the hardware technology solution into a software API that satisfies the business' requirements.

## Requirements: User Stories

###New machine without gumballs
As a gumball customer
I want to be told the machine is sold out
So that I can avoid losing my money or expecting a gumball

* __Initial State__

    Given I have a new gumball machine
    Then it is empty
    And it has no quarter
    And the display reads "Sorry, the machine is sold out"

* __Initial State, Insert Quarter__
    Given I have a new gumball machine
    When I insert a quarter
    Then it returns my quarter
    And it has no quarter
    And the display reads "There are no Gumballs, please pick up your Quarter"

* __Initial State, Eject Quarter__
    Given I have a new gumball machine
    When I eject a quarter
    Then it has no quarter
    And the display reads "This is not a Slot machine"

* __Initial State, Turn Crank__
    Given I have a new gumball machine
    When I turn the crank
    Then it has no quarter
    And the display reads "There are no Gumballs"

* __Initial State, Take Gumball__
    Given I have a new gumball machine
    When I try to take a gumball
    Then it has no quarter
    And the display reads "I can't give you what I don't have"

###New machine with gumballs
As a gumball customer
I want to be told the machine needs a quarter
So that I can avoid expecting a gumball or losing my money

* __Refill Initial State__
    Given I have a new gumball machine
    When I refill it
    Then it is not empty
    And it has no quarter
    And the display reads "Quarter for a Gumball"

* __Refill, Insert Quarter__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    Then it has a quarter
    And the display reads "Turn the Crank for a Gumball"

* __Refill, Eject Quarter__
    Given I have a new gumball machine
    When I refill it
    And I eject a quarter
    Then it has no quarter
    And the display reads "You haven't inserted a Quarter yet"

* __Refill, Turn Crank__
    Given I have a new gumball machine
    When I refill it
    And I turn the crank
    Then it has no quarter
    And the display reads "You need to pay first"

* __Refill, Take Gumball__
    Given I have a new gumball machine
    When I refill it
    And I try to take a gumball
    Then it has no quarter
    And the display reads "Quarter first, then a Gumball"

### Insert a quarter
In order to get a gumball
As a paid gumball customer
I want to be told what to do after paying

* __Quarter Initial State__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    Then it is not empty
    And it has a quarter
    And the display reads "Turn the Crank for a Gumball"

* __Quarter, Insert Another Quarter__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I insert a quarter
    Then it is not empty
    And it returns my quarter
    And it has a quarter
    And the display reads "You can't insert another Quarter"

* __Quarter, Eject Quarter__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I eject a quarter
    Then it is not empty
    And it returns my quarter
    And it has no quarter
    And the display reads "Pick up your Quarter from the tray"

* __Quarter, Turn Crank__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is dispensed
    Then it is not empty
    And it has no quarter
    And the display reads "A Gumball is on its way!"

* __Quarter, Take Gumball__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I try to take a gumball
    Then it is not empty
    And it has a quarter
    And the display reads "You need to turn the crank first"

###Turn the crank
As a paid gumball customer
I want to be told what to do after I crank the machine
So that I get a gumball

* __Crank Initial State__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is dispensed
    Then it is not empty
    And it has no quarter
    And the display reads "A Gumball is on its way!"

* __Crank Sold Out__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is not dispensed
    Then it is empty
    And it returns my quarter
    And it has no quarter
    And the display reads "There are no Gumballs, please pick up your Quarter"

* __Crank, Insert Quarter__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is dispensed
    And I insert a quarter
    Then it is not empty
    And it has no quarter
    And the display reads "Please wait, we're already giving you a Gumball"

* __Crank, Eject Quarter__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is dispensed
    And I eject a quarter
    Then it is not empty
    And it has no quarter
    And the display reads "Sorry, you already turned the Crank"

* __Crank, Turn Crank__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is dispensed
    And I turn the crank
    Then it is not empty
    And it has no quarter
    And the display reads "Turning again doesn't get you another Gumball"

* __Crank, Take Gumball__
    Given I have a new gumball machine
    When I refill it
    And I insert a quarter
    And I turn the crank
    And a gumball is dispensed
    And I try to take a gumball
    Then it is not empty
    And it has no quarter
    And the display reads "Quarter for a Gumball"

## Technology Solutions

 * [F#](./f%23/src)
 * Java (to-do, maybe)
 * C# (to-do, maybe)
 * Javascript (to-do, maybe)
 * Python (to-do, maybe)
