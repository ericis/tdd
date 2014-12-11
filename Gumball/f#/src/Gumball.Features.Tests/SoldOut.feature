Feature: SoldOutFeature
	In order to avoid losing my money or expecting a gumball
	As a gumball customer
	I want to be told the machine is sold out

Scenario: Initial State
	Given I have a new gumball machine
	Then it is empty
	And it has no quarter
	And the display reads "Sorry, the machine is sold out"

Scenario: Initial State, Insert Quarter
	Given I have a new gumball machine
	When I insert a quarter
	Then it returns my quarter
	And it has no quarter
	And the display reads "There are no Gumballs, please pick up your Quarter"

Scenario: Initial State, Eject Quarter
	Given I have a new gumball machine
	When I eject a quarter
	Then it has no quarter
	And the display reads "This is not a Slot machine"

Scenario: Initial State, Turn Crank
	Given I have a new gumball machine
	When I turn the crank
	Then it has no quarter
	And the display reads "There are no Gumballs"

Scenario: Initial State, Take Gumball
	Given I have a new gumball machine
	When I try to take a gumball
	Then it has no quarter
	And the display reads "I can't give you what I don't have"