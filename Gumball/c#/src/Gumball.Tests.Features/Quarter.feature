﻿Feature: QuarterFeature
	In order to get a gumball
	As a paid gumball customer
	I want to be told what to do after paying

Scenario: Quarter Initial State
	Given I have a new gumball machine
	And I refill it
	When I insert a quarter
	Then the display reads "Turn the Crank for a Gumball"

Scenario: Quarter, Insert Another Quarter
	Given I have a new gumball machine
	And I refill it
	And I insert a quarter
	When I insert a quarter
	Then it returns my quarter
	And the display reads "You can't insert another Quarter"

Scenario: Quarter, Eject Quarter
	Given I have a new gumball machine
	And I refill it
	And I insert a quarter
	When I eject a quarter
	Then it returns my quarter
	And the display reads "Pick up your Quarter from the tray"

Scenario: Quarter, Turn Crank
	Given I have a new gumball machine
	And I refill it
	And I insert a quarter
	And I turn the crank
	When a gumball is dispensed
	Then the display reads "A Gumball is on its way!"

Scenario: Quarter, Take Gumball
	Given I have a new gumball machine
	And I refill it
	And I insert a quarter
	When I try to take a gumball
	Then the display reads "You need to turn the crank first"