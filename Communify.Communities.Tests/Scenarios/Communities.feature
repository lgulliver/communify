Feature: Communities
	In order to manage events
	As a Communifier
	I want to be able to manage my communities

Background: 
    Given a communifier
	And a non-communifier

@mytag
Scenario: Create a community
	Given a valid new community
	When the community is posted
	Then the community should be created
