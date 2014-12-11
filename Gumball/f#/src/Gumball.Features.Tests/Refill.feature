﻿Feature: RefillFeature
	In order to avoid expecting a gumball or losing my money
	As a gumball customer
	I want to be told the machine needs a quarter

Scenario: Refill Initial State
	Given I have a new gumball machine
	When I refill it
	Then it is not empty
	And it has no quarter
	And the display reads "Quarter for a Gumball"

Scenario: Refill, Insert Quarter
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	Then it has a quarter
	And the display reads "Turn the Crank for a Gumball"

Scenario: Refill, Eject Quarter
	Given I have a new gumball machine
	When I refill it
	And I eject a quarter
	Then it has no quarter
	And the display reads "You haven't inserted a Quarter yet"

Scenario: Refill, Turn Crank
	Given I have a new gumball machine
	When I refill it
	And I turn the crank
	Then it has no quarter
	And the display reads "You need to pay first"

Scenario: Refill, Take Gumball
	Given I have a new gumball machine
	When I refill it
	And I try to take a gumball
	Then it has no quarter
	And the display reads "Quarter first, then a Gumball"