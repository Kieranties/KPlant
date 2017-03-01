using KPlant.Model;
using KPlant.Sequence.Model;
using Xunit.Abstractions;

namespace KPlant.Sequence.IntegrationTests
{
    public class DslIntegrationTests : IntegrationTests
    {
        public DslIntegrationTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override SequenceDiagram ActivationTest()
        {
            var user = Participant.Called("User");
            var a = Participant.Called("A");
            var b = Participant.Called("B");
            var c = Participant.Called("C");

            return SequenceDiagram.Of(
                user,
                Message.Between(user, a, "DoWork"),
                ActivationStatus.Activate(a),
                Message.Between(a, b, "<< createRequest >>"),
                ActivationStatus.Activate(b),
                Message.Between(b, c, "DoWork"),
                ActivationStatus.Activate(c),
                Message.Between(c, b, "WorkDone").Arrow(Arrow.Dotted()),
                ActivationStatus.Destroy(c),
                Message.Between(b, a, "RequestCreated").Arrow(Arrow.Dotted()),
                ActivationStatus.Deactivate(b),
                Message.Between(a, user, "Done"),
                ActivationStatus.Deactivate(a)
            );
        }

        protected override SequenceDiagram ArrowColourTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Message.Between(bob, alice, "hello").Arrow(Arrow.Default().Colour("red")),
                Message.Between(alice, bob, "ok").Arrow(Arrow.Dotted().Colour("0000FF"))
            );
        }

        protected override SequenceDiagram ArrowStyleTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Message.Between(bob, alice).Arrow(new Arrow { Head = new ArrowHead { Status = ArrowHeadStatus.Fail } }),
                Message.Between(bob, alice),
                Message.Between(bob, alice).Arrow(new Arrow { Thickness = ArrowThickness.Thin }),
                Message.Between(bob, alice).Arrow(new Arrow { Head = new ArrowHead { Parts = ArrowHeadParts.Top } }),
                Message.Between(bob, alice).Arrow(new Arrow { Thickness = ArrowThickness.Thin, Head = new ArrowHead { Parts = ArrowHeadParts.Top } }),
                Message.Between(bob, alice).Arrow(new Arrow { Type = ArrowType.Dotted, Thickness = ArrowThickness.Thin, Head = new ArrowHead { Parts = ArrowHeadParts.Bottom } }),
                Message.Between(bob, alice).Arrow(new Arrow { Head = new ArrowHead { Status = ArrowHeadStatus.Success } }),
                Message.Between(bob, alice).Arrow(new Arrow { Type = ArrowType.Dotted, Thickness = ArrowThickness.Thin, Head = new ArrowHead { Parts = ArrowHeadParts.Top, Status = ArrowHeadStatus.Success } }),
                Message.Between(bob, alice).Arrow(new Arrow { FromHead = new ArrowHead() }),
                Message.Between(bob, alice).Arrow(new Arrow { Head = new ArrowHead { Status = ArrowHeadStatus.Success }, FromHead = new ArrowHead() }),
                Message.Between(bob, alice).Arrow(new Arrow { Thickness = ArrowThickness.Thin, Head = new ArrowHead { Status = ArrowHeadStatus.Success, Parts = ArrowHeadParts.Top }, FromHead = new ArrowHead() }),
                Message.Between(bob, alice).Arrow(new Arrow { Type = ArrowType.Dotted, Head = new ArrowHead { Status = ArrowHeadStatus.Success }, FromHead = new ArrowHead { Status = ArrowHeadStatus.Success } })
            );
        }

        protected override SequenceDiagram BasicTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Message.Between(alice, bob, "Authentication Request"),
                Message.Between(bob, alice, "Authentication Response").Arrow(Arrow.Dotted()),
                Message.Between(alice, bob, "Another authentication Request"),
                Message.Between(bob, alice, "Another authentication Response").Arrow(Arrow.Dotted())
            );
        }

        protected override SequenceDiagram ColourAndAliasingTest()
        {
            var bob = Participant.Actor("Bob").Colour("red");
            var alice = Participant.Called("Alice");
            var l = Participant.Called("L", "I have a really\nlong name").Colour("99FF99");

            return SequenceDiagram.Of(
                bob, alice, l,
                Message.Between(alice, bob, "Authentication Request"),
                Message.Between(bob, alice, "Authentication Response"),
                Message.Between(bob, l, "Log transaction")
            );
        }

        protected override SequenceDiagram DeclaringParticipantTest()
        {
            var p1 = Participant.Actor("Foo1");
            var p2 = Participant.Boundary("Foo2");
            var p3 = Participant.Control("Foo3");
            var p4 = Participant.Entity("Foo4");
            var p5 = Participant.Database("Foo5");

            return SequenceDiagram.Of(
                p1, p2, p3, p4, p5,
                Message.Between(p1, p2, "To boundary"),
                Message.Between(p1, p3, "To control"),
                Message.Between(p1, p4, "To entity"),
                Message.Between(p1, p5, "To database")
            );
        }

        protected override SequenceDiagram DelayTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Message.Between(alice, bob, "Authentication Request"),
                Separator.Delay(),
                Message.Between(bob, alice, "Authentication Response").Arrow(Arrow.Dotted()),
                Separator.Delay("5 minutes later"),
                Message.Between(bob, alice, "Bye !").Arrow(Arrow.Dotted())
            );
        }

        protected override SequenceDiagram DividerTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Separator.Divider("Initialization"),
                Message.Between(alice, bob, "Authentication Request"),
                Message.Between(bob, alice, "Authentication Response").Arrow(Arrow.Dotted()),
                Separator.Divider("Repetition"),
                Message.Between(alice, bob, "Another authentication Request"),
                Message.Between(bob, alice, "Another authentication Response").Arrow(Arrow.Dotted())
            );
        }

        protected override SequenceDiagram GroupingTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");
            var log = Participant.Called("Log");

            return SequenceDiagram.Of(
                Message.Between(alice, bob, "Authentication Request"),
                Group.Alt(Message.Between(bob, alice, "Authentication Accepted")).Label("successful case")
                    .Else(
                        Group.Of(
                            Message.Between(bob, alice, "Authentication Failure"),
                            Group.Of(
                                Message.Between(alice, log, "Log attack start"),
                                Group.Loop(Message.Between(alice, bob, "DNS Attack"))
                                    .Label("1000 times"),
                                Message.Between(alice, log, "Log attack end")
                            ).Label("My own label")
                        ).Label("some kind of failure"),
                        Group.Of(Message.Between(bob, alice, "Please repeat"))
                            .Label("Another type of failure")
                )
            );
        }

        protected override SequenceDiagram MessageSequenceNumberingFormatTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Numbering.Start().WithFormat("<b>[000]"),
                Message.Between(bob, alice, "Authentication Request"),
                Message.Between(alice, bob, "Authentication Response"),
                Numbering.Start(15).WithFormat("<b>(<u>##</u>)"),
                Message.Between(bob, alice, "Another authentication Request"),
                Message.Between(alice, bob, "Another authentication Response"),
                Numbering.Start(40, 10).WithFormat("<font color=red><b>Message 0  "),
                Message.Between(bob, alice, "Yet another authentication Request"),
                Message.Between(alice, bob, "Yet another authentication Response")
            );
        }

        protected override SequenceDiagram MessageSequenceNumberingIncrementTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Numbering.Start(),
                Message.Between(bob, alice, "Authentication Request"),
                Message.Between(alice, bob, "Authentication Response"),
                Numbering.Start(15),
                Message.Between(bob, alice, "Another authentication Request"),
                Message.Between(alice, bob, "Another authentication Response"),
                Numbering.Start(40, 10),
                Message.Between(bob, alice, "Yet another authentication Request"),
                Message.Between(alice, bob, "Yet another authentication Response")
            );
        }

        protected override SequenceDiagram MessageSequenceNumberingStopResumeTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Numbering.Start(10, 10).WithFormat("<b>[000]"),
                Message.Between(bob, alice, "Authentication Request"),
                Message.Between(alice, bob, "Authentication Response"),
                Numbering.Stop(),
                Message.Between(bob, alice, "dummy"),
                Numbering.Resume().WithFormat("<font color=red><b>Message 0  "),
                Message.Between(bob, alice, "Yet another authentication Request"),
                Message.Between(alice, bob, "Yet another authentication Response"),
                Numbering.Stop(),
                Message.Between(bob, alice, "dummy"),
                Numbering.Resume(1).WithFormat("<font color=blue><b>Message 0  "),
                Message.Between(bob, alice, "Yet another authentication Request"),
                Message.Between(alice, bob, "Yet another authentication Response")
            );
        }

        protected override SequenceDiagram MessageSequenceNumberingTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Numbering.Start(),
                Message.Between(bob, alice, "Authentication Request"),
                Message.Between(alice, bob, "Authentication Response")
            );
        }

        protected override SequenceDiagram MessageToSelfTest()
        {
            var alice = Participant.Called("Alice");

            return SequenceDiagram.Of(
                Message.Between(alice, alice, "This is a signal to self.\nIt also demonstrates\nmultiline \ntext")
            );
        }

        protected override SequenceDiagram NonLetterParticipantsTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob()");
            var @long = Participant.Called("Long", "This is very\nlong");

            return SequenceDiagram.Of(
                Message.Between(alice, bob, "Hello"),
                Message.Between(bob, @long),
                Message.Between(@long, bob, "ok")
            );
        }

        protected override SequenceDiagram RefBlockTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Actor("Bob");

            return SequenceDiagram.Of(
                alice, bob,
                Ref.Over("init", alice, bob),
                Message.Between(alice, bob, "hello"),
                Ref.Over("This can be on\nseveral lines", bob)
            );
        }

        protected override SequenceDiagram SpaceTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Message.Between(alice, bob, "message 1"),
                Message.Between(bob, alice, "ok").Arrow(Arrow.Dotted()),
                Separator.Space(),
                Message.Between(alice, bob, "message 2"),
                Message.Between(bob, alice, "ok").Arrow(Arrow.Dotted()),
                Separator.Space(45),
                Message.Between(alice, bob, "message 3"),
                Message.Between(bob, alice, "ok").Arrow(Arrow.Dotted())
            );
        }

        protected override SequenceDiagram SplittingDiagramsTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            return SequenceDiagram.Of(
                Message.Between(alice, bob, "message 1"),
                Message.Between(alice, bob, "message 2"),
                Separator.Page(),
                Message.Between(alice, bob, "message 3"),
                Message.Between(alice, bob, "message 4"),
                Separator.Page("A title for the\nlast page"),
                Message.Between(alice, bob, "message 5"),
                Message.Between(alice, bob, "message 6")
            );
        }
    }
}