Feature: QuarterFeature
	In order to get a gumball
	As a paid gumball customer
	I want to be told what to do after paying

Scenario: Quarter Initial State
	Given I have a new gumball machine
	When I refill it
	And I insert a quarter
	Then it is not empty
	And it has a quarter
	And the display reads "Turn the Crank for a Gumball"