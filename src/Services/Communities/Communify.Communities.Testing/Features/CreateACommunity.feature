Feature: CreateACommunity



Scenario: Create a Community
	Given a community has formed
	When the community is created 
	Then the community creation was successful 
	And the community can be accessed

Scenario: Validate a Community
 Given a community has formed
 And the community is called <name>
 And the community is described as <description>
 And the community has been given an id of <id>
 When the community is created
 Then the community creation has a result of <result>
 Examples: 
 | name      | description      | id       | result      |
 | good name | good description | <guid>   | Created     |
 | <null>    | good description | <guid>   | Bad Request |
 | good name | <null>           | <guid>   | Bad Request |
 #| good name | good description | a string | Bad Request |
 #| good name | good description | <null>   | Bad Request |
 | good name | good description | a string | Server Error |
 | good name | good description | <null>   | Server Error |