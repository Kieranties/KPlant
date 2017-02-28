using System;
using System.Collections.Generic;
using System.Text;

// Expectations from http://plantuml.com/PlantUML_Language_Reference_Guide.pdf

namespace KPlant.Sequence.IntegrationTests
{
    public class Expectations
    {

        public static string Basic = @"@startuml
Alice -> Bob : Authentication Request
Bob --> Alice : Authentication Response
Alice -> Bob : Another authentication Request
Bob --> Alice : Another authentication Response
@enduml
";

        public static string DeclaringParticipant = @"@startuml
actor Foo1
boundary Foo2
control Foo3
entity Foo4
database Foo5
Foo1 -> Foo2 : To boundary
Foo1 -> Foo3 : To control
Foo1 -> Foo4 : To entity
Foo1 -> Foo5 : To database
@enduml
";

        public static string ColourAndAliasing = @"@startuml
actor Bob #red
participant Alice
participant ""I have a really\nlong name"" as L #99FF99
Alice -> Bob : Authentication Request
Bob -> Alice : Authentication Response
Bob -> L : Log transaction
@enduml
";

        public static string NonLetterParticipants = @"@startuml
Alice -> ""Bob()"" : Hello
""Bob()"" -> ""This is very\nlong"" as Long
Long --> ""Bob()"" : ok
@enduml
";

        public static string MessageToSelf = @"@startuml
Alice -> Alice : This is a signal to self.\nIt also demonstrates\nmultiline \ntext
@enduml
";


        //TODO: support double ended arrows
        /*
         * Bob <-> Alice
         * Bob <->o Alice
         */
        public static string ArrowStyle = @"@startuml
Bob ->x Alice
Bob -> Alice
Bob ->> Alice
Bob -\ Alice
Bob -\\ Alice
Bob --// Alice
Bob ->o Alice
Bob --\\o Alice
@enduml
";

        public static string ArrowColour = @"@startuml
Bob -[#red]> Alice : hello
Alice -[#0000FF]-> Bob : ok
@enduml
";

        public static string MessageSequenceNumbering = @"@startuml
autonumber
Bob -> Alice : Authentication Request
Alice -> Bob : Authentication Response
@enduml
";

        public static string MessageSequenceNumberingIncrement = @"@startuml
autonumber
Bob -> Alice : Authentication Request
Alice -> Bob : Authentication Response
autonumber 15
Bob -> Alice : Another authentication Request
Alice -> Bob : Another authentication Response
autonumber 40 10
Bob -> Alice : Yet another authentication Request
Alice -> Bob : Yet another authentication Response
@enduml
";

        public static string MessageSequenceNumberingFormat = @"@startuml
autonumber ""<b>[000]""
Bob -> Alice : Authentication Request
Alice -> Bob : Authentication Response
autonumber 15 ""<b>(<u>##</u>)""
Bob -> Alice : Another authentication Request
Alice -> Bob : Another authentication Response
autonumber 40 10 ""<font color=red><b>Message 0  ""
Bob -> Alice : Yet another authentication Request
Alice -> Bob : Yet another authentication Response
@enduml
";

        public static string MessageSequenceNumberingStopResume = @"@startuml
autonumber 10 10 ""<b>[000]""
Bob -> Alice : Authentication Request
Alice -> Bob : Authentication Response
autonumber stop
Bob -> Alice : dummy
autonumber resume ""<font color=red><b>Message 0  ""
Bob -> Alice : Yet another authentication Request
Alice -> Bob : Yet another authentication Response
autonumber stop
Bob -> Alice : dummy
autonumber resume 1 ""<font color=blue><b>Message 0  ""
Bob -> Alice : Yet another authentication Request
Alice -> Bob : Yet another authentication Response
@enduml
";

        public static string SplittingDiagrams = @"@startuml
Alice -> Bob : message 1
Alice -> Bob : message 2
newpage
Alice -> Bob : message 3
Alice -> Bob : message 4
newpage A title for the\nlast page
Alice -> Bob : message 5
Alice -> Bob : message 6
@enduml
";


        /// <summary>
        /// This verbatim string contains tabs!
        /// </summary>
        public const string Grouping = @"@startuml
Alice -> Bob : Authentication Request
alt successful case
	Bob -> Alice : Authentication Accepted
else some kind of failure
	Bob -> Alice : Authentication Failure
	group My own label
		Alice -> Log : Log attack start
		loop 1000 times
			Alice -> Bob : DNS Attack
		end
		Alice -> Log : Log attack end
	end
else Another type of failure
	Bob -> Alice : Please repeat
end
@enduml
";

        /// <summary>
        /// This verbatim string contains spaces!
        /// </summary>
        public const string GroupingWithSpaces = @"@startuml
Alice -> Bob : Authentication Request
alt successful case
    Bob -> Alice : Authentication Accepted
else some kind of failure
    Bob -> Alice : Authentication Failure
    group My own label
        Alice -> Log : Log attack start
        loop 1000 times
            Alice -> Bob : DNS Attack
        end
        Alice -> Log : Log attack end
    end
else Another type of failure
    Bob -> Alice : Please repeat
end
@enduml
";


        public static string Divider = @"@startuml
== Initialization ==
Alice -> Bob : Authentication Request
Bob --> Alice : Authentication Response
== Repetition ==
Alice -> Bob : Another authentication Request
Bob --> Alice : Another authentication Response
@enduml
";


        public static string Ref = @"@startuml
participant Alice
actor Bob
ref over Alice, Bob : init
Alice -> Bob : hello
ref over Bob : This can be on\nseveral lines
@enduml
";
        public static string Delay = @"@startuml
Alice -> Bob : Authentication Request
...
Bob --> Alice : Authentication Response
...5 minutes later...
Bob --> Alice : Bye !
@enduml
";
        public static string Space = @"@startuml
Alice -> Bob : message 1
Bob --> Alice : ok
|||
Alice -> Bob : message 2
Bob --> Alice : ok
||45||
Alice -> Bob : message 3
Bob --> Alice : ok
@enduml
";
        public static string Activation = @"@startuml
participant User
User -> A : DoWork
activate A
A -> B : << createRequest >>
activate B
B -> C : DoWork
activate C
C --> B : WorkDone
destroy C
B --> A : RequestCreated
deactivate B
A -> User : Done
deactivate A
@enduml
";
    }
}
