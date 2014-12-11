Feature: CrankFeature
	In order to get a gumball
	As a paid gumball customer
	I want to be told what to do after I crank the machine

Scenario: Crank Initial State
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	And I turn the crank
	And a gumball is dispensed
	Then it is not empty
	And it has no quarter
	And the display reads "A Gumball is on its way!"

Scenario: Crank Sold Out
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	And I turn the crank
	And a gumball is not dispensed
	Then it is empty
	And it returns my quarter
	And it has no quarter
	And the display reads "There are no Gumballs, please pick up your Quarter"

Scenario: Crank, Insert Quarter
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	And I turn the crank
	And a gumball is dispensed
	And I insert a quarter
	Then it is not empty
	And it has no quarter
	And the display reads "Please wait, we're already giving you a Gumball"

Scenario: Crank, Eject Quarter
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	And I turn the crank
	And a gumball is dispensed
	And I eject a quarter
	Then it is not empty
	And it has no quarter
	And the display reads "Sorry, you already turned the Crank"

Scenario: Crank, Turn Crank
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	And I turn the crank
	And a gumball is dispensed
	And I turn the crank
	Then it is not empty
	And it has no quarter
	And the display reads "Turning again doesn't get you another Gumball"

Scenario: Crank, Take Gumball
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	And I turn the crank
	And a gumball is dispensed
	And I try to take a gumball
	Then it is not empty
	And it has no quarter
	And the display reads "Quarter for a Gumball"