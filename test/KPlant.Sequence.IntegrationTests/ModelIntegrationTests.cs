using KPlant.Sequence.Model;
using Xunit.Abstractions;

namespace KPlant.Sequence.IntegrationTests
{
    public class ModelIntegrationTests : IntegrationTests
    {
        public ModelIntegrationTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override SequenceDiagram ActivationTest()
        {
            var user = new Participant("User");
            var a = new Participant("A");
            var b = new Participant("B");
            var c = new Participant("C");

            return new SequenceDiagram {
                user,
                new Message(user, a, "DoWork"),
                new ActivationStatus(a, ActivationState.Activate),
                new Message(a, b, "<< createRequest >>"),
                new ActivationStatus(b, ActivationState.Activate),
                new Message(b, c, "DoWork"),
                new ActivationStatus(c, ActivationState.Activate),
                new Message(c, b, "WorkDone") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new ActivationStatus(c, ActivationState.Destroy),
                new Message(b, a, "RequestCreated") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new ActivationStatus(b, ActivationState.Deactivate),
                new Message(a, user, "Done"),
                new ActivationStatus(a, ActivationState.Deactivate)
            };
        }

        protected override SequenceDiagram ArrowColourTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Message(bob, alice, "hello") { Arrow = new Arrow { Colour = "red" } },
                new Message(alice, bob, "ok") { Arrow = new Arrow { Colour = "0000FF", Type = ArrowType.Dotted } }
            };
        }

        protected override SequenceDiagram ArrowStyleTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return new SequenceDiagram {
                new Message(bob, alice) { Arrow = new Arrow { Head = new ArrowHead { Status = ArrowHeadStatus.Fail } } },
                new Message(bob, alice),
                new Message(bob, alice) { Arrow = new Arrow { Thickness = ArrowThickness.Thin } },
                new Message(bob, alice) { Arrow = new Arrow { Head = new ArrowHead { Parts = ArrowHeadParts.Top } } },
                new Message(bob, alice) { Arrow = new Arrow { Thickness = ArrowThickness.Thin, Head = new ArrowHead { Parts = ArrowHeadParts.Top } } },
                new Message(bob, alice) { Arrow = new Arrow { Type = ArrowType.Dotted, Thickness = ArrowThickness.Thin, Head = new ArrowHead { Parts = ArrowHeadParts.Bottom } } },
                new Message(bob, alice) { Arrow = new Arrow { Head = new ArrowHead { Status = ArrowHeadStatus.Success } } },
                new Message(bob, alice) { Arrow = new Arrow { Type = ArrowType.Dotted, Thickness = ArrowThickness.Thin, Head = new ArrowHead { Parts = ArrowHeadParts.Top, Status = ArrowHeadStatus.Success } } },
                new Message(bob, alice).Arrow(new Arrow { FromHead = new ArrowHead() }),
                new Message(bob, alice) { Arrow = new Arrow { Head = new ArrowHead { Status = ArrowHeadStatus.Success }, FromHead = new ArrowHead() } },
                new Message(bob, alice) { Arrow = new Arrow { Thickness = ArrowThickness.Thin, Head = new ArrowHead { Status = ArrowHeadStatus.Success, Parts = ArrowHeadParts.Top }, FromHead = new ArrowHead() } },
                new Message(bob, alice) { Arrow = new Arrow { Type = ArrowType.Dotted, Head = new ArrowHead { Status = ArrowHeadStatus.Success }, FromHead = new ArrowHead { Status = ArrowHeadStatus.Success } } }
            };
        }

        protected override SequenceDiagram BasicTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram
            {
                new Message(alice, bob, "Authentication Request"),
                new Message(bob, alice, "Authentication Response") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new Message(alice, bob, "Another authentication Request"),
                new Message(bob, alice, "Another authentication Response") { Arrow = new Arrow { Type = ArrowType.Dotted } }
            };
        }

        protected override SequenceDiagram ColourAndAliasingTest()
        {
            var bob = new Participant("Bob", ParticipantType.Actor) { Colour = "red" };
            var alice = new Participant("Alice");
            var l = new Participant("L", "I have a really\nlong name") { Colour = "99FF99" };

            return new SequenceDiagram {
                bob, alice, l,
                new Message(alice, bob, "Authentication Request"),
                new Message(bob, alice, "Authentication Response"),
                new Message(bob, l, "Log transaction")
            };
        }

        protected override SequenceDiagram DeclaringParticipantTest()
        {
            var p1 = new Participant("Foo1", ParticipantType.Actor);
            var p2 = new Participant("Foo2", ParticipantType.Boundary);
            var p3 = new Participant("Foo3", ParticipantType.Control);
            var p4 = new Participant("Foo4", ParticipantType.Entity);
            var p5 = new Participant("Foo5", ParticipantType.Database);

            return new SequenceDiagram {
                p1, p2, p3, p4, p5,
                new Message(p1, p2, "To boundary"),
                new Message(p1, p3, "To control"),
                new Message(p1, p4, "To entity"),
                new Message(p1, p5, "To database")
            };
        }

        protected override SequenceDiagram DelayTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Message(alice, bob, "Authentication Request"),
                new Delay(),
                new Message(bob, alice, "Authentication Response") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new Delay("5 minutes later"),
                new Message(bob, alice, "Bye !") { Arrow = new Arrow { Type = ArrowType.Dotted } }
            };
        }

        protected override SequenceDiagram DividerTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Divider("Initialization"),
                new Message(alice, bob, "Authentication Request"),
                new Message(bob, alice, "Authentication Response") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new Divider("Repetition"),
                new Message(alice, bob, "Another authentication Request"),
                new Message(bob, alice, "Another authentication Response") { Arrow = new Arrow { Type = ArrowType.Dotted } }
            };
        }

        protected override SequenceDiagram GroupingTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");
            var log = new Participant("Log");

            return new SequenceDiagram {
                new Message(alice, bob, "Authentication Request"),
                new Group(GroupType.Alt, "successful case")
                {
                    Elements = { new Message(bob, alice, "Authentication Accepted") },
                    Else =
                    {
                        new Group("some kind of failure")
                        {
                            Message.Between(bob, alice, "Authentication Failure"),
                            new Group("My own label")
                            {
                                new Message(alice, log, "Log attack start"),
                                new Group(GroupType.Loop, "1000 times")
                                {
                                    new Message(alice, bob, "DNS Attack")
                                },
                                new Message(alice, log, "Log attack end")
                            }
                        },
                        new Group("Another type of failure")
                        {
                            new Message(bob, alice, "Please repeat")
                        }
                    }
                }
            };
        }

        protected override SequenceDiagram MessageSequenceNumberingFormatTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Numbering(NumberCommand.Start) { Format = "<b>[000]" },
                new Message(bob, alice, "Authentication Request"),
                new Message(alice, bob, "Authentication Response"),
                new Numbering(NumberCommand.Start, 15) { Format = "<b>(<u>##</u>)" },
                new Message(bob, alice, "Another authentication Request"),
                new Message(alice, bob, "Another authentication Response"),
                new Numbering(NumberCommand.Start, 40, 10) { Format = "<font color=red><b>Message 0  " },
                new Message(bob, alice, "Yet another authentication Request"),
                new Message(alice, bob, "Yet another authentication Response")
            };
        }

        protected override SequenceDiagram MessageSequenceNumberingIncrementTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Numbering(NumberCommand.Start),
                new Message(bob, alice, "Authentication Request"),
                new Message(alice, bob, "Authentication Response"),
                new Numbering(NumberCommand.Start, 15),
                new Message(bob, alice, "Another authentication Request"),
                new Message(alice, bob, "Another authentication Response"),
                new Numbering(NumberCommand.Start, 40, 10),
                new Message(bob, alice, "Yet another authentication Request"),
                new Message(alice, bob, "Yet another authentication Response")
            };
        }

        protected override SequenceDiagram MessageSequenceNumberingStopResumeTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Numbering(NumberCommand.Start, 10, 10) { Format = "<b>[000]" },
                new Message(bob, alice, "Authentication Request"),
                new Message(alice, bob, "Authentication Response"),
                new Numbering(NumberCommand.Stop),
                new Message(bob, alice, "dummy"),
                new Numbering(NumberCommand.Resume) { Format = "<font color=red><b>Message 0  " },
                new Message(bob, alice, "Yet another authentication Request"),
                new Message(alice, bob, "Yet another authentication Response"),
                new Numbering(NumberCommand.Stop),
                new Message(bob, alice, "dummy"),
                new Numbering(NumberCommand.Resume, 1) { Format = "<font color=blue><b>Message 0  " },
                new Message(bob, alice, "Yet another authentication Request"),
                new Message(alice, bob, "Yet another authentication Response")
            };
        }

        protected override SequenceDiagram MessageSequenceNumberingTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Numbering(NumberCommand.Start),
                new Message(bob, alice, "Authentication Request"),
                new Message(alice, bob, "Authentication Response")
            };
        }

        protected override SequenceDiagram MessageToSelfTest()
        {
            var alice = new Participant("Alice");

            return new SequenceDiagram {
                new Message(alice, alice, "This is a signal to self.\nIt also demonstrates\nmultiline \ntext")
            };
        }

        protected override SequenceDiagram NonLetterParticipantsTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");
            var @long = new Participant("Long", "This is very\nlong");

            return new SequenceDiagram {
                new Message(alice, bob, "Hello"),
                new Message(bob, @long),
                new Message(@long, bob, "ok")
            };
        }

        protected override SequenceDiagram RefBlockTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob", ParticipantType.Actor);

            return new SequenceDiagram {
                alice, bob,
                new Ref("init", alice, bob),
                new Message(alice, bob, "hello"),
                new Ref("This can be on\nseveral lines", bob)
            };
        }

        protected override SequenceDiagram SpaceTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Message(alice, bob, "message 1"),
                new Message(bob, alice, "ok") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new Space(),
                new Message(alice, bob, "message 2"),
                new Message(bob, alice, "ok") { Arrow = new Arrow { Type = ArrowType.Dotted } },
                new Space(45),
                new Message(alice, bob, "message 3"),
                new Message(bob, alice, "ok") { Arrow = new Arrow { Type = ArrowType.Dotted } }
            };
        }

        protected override SequenceDiagram SplittingDiagramsTest()
        {
            var alice = new Participant("Alice");
            var bob = new Participant("Bob");

            return new SequenceDiagram {
                new Message(alice, bob, "message 1"),
                new Message(alice, bob, "message 2"),
                new Page(),
                new Message(alice, bob, "message 3"),
                new Message(alice, bob, "message 4"),
                new Page("A title for the\nlast page"),
                new Message(alice, bob, "message 5"),
                new Message(alice, bob, "message 6")
            };
        }
    }
}