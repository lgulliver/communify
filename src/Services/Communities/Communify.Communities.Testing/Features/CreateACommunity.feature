Feature: CreateACommunity



Scenario: Create a Community
	Given a community has formed
	When the community is created 
	Then the community creation was successful 
	And the community can be accessed
