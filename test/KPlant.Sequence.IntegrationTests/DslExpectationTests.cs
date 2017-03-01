using KPlant.Model;
using KPlant.Rendering;
using KPlant.Sequence.Model;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace KPlant.Sequence.IntegrationTests
{
    public class DslExpectationTests
    {
        private readonly ITestOutputHelper _output;

        public DslExpectationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void Basic()
        {

            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Message.Between(alice, bob, "Authentication Request"),
                Message.Between(bob, alice, "Authentication Response").WithArrow(Arrow.Dotted),
                Message.Between(alice, bob, "Another authentication Request"),
                Message.Between(bob, alice, "Another authentication Response").WithArrow(Arrow.Dotted)
            );

            await AssertDiagram(diagram, Expectations.Basic);
        }

        [Fact]
        public async void DeclaringParticipant()
        {
            var p1 = Participant.Actor("Foo1");
            var p2 = Participant.Boundary("Foo2");
            var p3 = Participant.Control("Foo3");
            var p4 = Participant.Entity("Foo4");
            var p5 = Participant.Database("Foo5");

            var diagram = SequenceDiagram.Of(
                p1,p2,p3,p4,p5,
                Message.Between(p1, p2,"To boundary"),
                Message.Between(p1, p3,"To control"),
                Message.Between(p1, p4,"To entity"),
                Message.Between(p1, p5,"To database")
            );

            await AssertDiagram(diagram, Expectations.DeclaringParticipant);
        }

        [Fact]
        public async void ColourAndAliasing()
        {
            var bob = Participant.Actor("Bob").WithColour("red");
            var alice = Participant.Called("Alice");
            var l = Participant.Called("L", "I have a really\nlong name").WithColour("99FF99");

            var diagram = SequenceDiagram.Of(            
                bob,alice,l,
                Message.Between(alice,bob,"Authentication Request"),
                Message.Between(bob,alice,"Authentication Response"),
                Message.Between(bob, l,"Log transaction")
            );

            await AssertDiagram(diagram, Expectations.ColourAndAliasing);
        }

        [Fact(Skip = "Lacking support late declaration of participants")]
        public async void NonLetterParticipants()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob()");
            var @long = Participant.Called("Long", "This is very\nlong");

            var diagram = SequenceDiagram.Of(
                Message.Between(alice, bob, "Hello"),
                Message.Between(bob, @long),
                Message.Between(@long, bob, "ok")
            );

            await AssertDiagram(diagram, Expectations.NonLetterParticipants);
        }

        [Fact]
        public async void MessageToSelf()
        {
            var alice = Participant.Called("Alice");
            var diagram = SequenceDiagram.Of(
                Message.Between(alice, alice, "This is a signal to self.\nIt also demonstrates\nmultiline \ntext")
            );

            await AssertDiagram(diagram, Expectations.MessageToSelf);
        }

        [Fact]
        public async void ArrowStyle()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");
            var diagram = SequenceDiagram.Of(
                Message.Between(bob, alice).WithArrow(new Arrow{ Head = new ArrowHead{ Status = ArrowHeadStatus.Fail } }),
                Message.Between(bob, alice),
                Message.Between(bob, alice).WithArrow(new Arrow{ Head = new ArrowHead{ Thickness = ArrowHeadThickness.Thin } }),
                Message.Between(bob, alice).WithArrow(new Arrow{ Head = new ArrowHead{ Parts = ArrowHeadParts.Top } }),
                Message.Between(bob, alice).WithArrow(new Arrow{ Head = new ArrowHead{ Parts = ArrowHeadParts.Top, Thickness = ArrowHeadThickness.Thin } }),
                Message.Between(bob, alice).WithArrow(new Arrow{ Type = ArrowType.Dotted, Head = new ArrowHead{ Parts = ArrowHeadParts.Bottom, Thickness = ArrowHeadThickness.Thin } }),
                Message.Between(bob, alice).WithArrow(new Arrow{ Head = new ArrowHead{ Status = ArrowHeadStatus.Success } }),
                Message.Between(bob, alice).WithArrow(new Arrow{ Type = ArrowType.Dotted, Head = new ArrowHead{ Parts = ArrowHeadParts.Top, Thickness = ArrowHeadThickness.Thin, Status = ArrowHeadStatus.Success } })
            );

            await AssertDiagram(diagram, Expectations.ArrowStyle);
        }

        [Fact]
        public async void ArrowColour()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Message.Between(bob, alice, "hello").WithArrow(new Arrow{ Colour = "red" }),
                Message.Between(alice, bob, "ok").WithArrow(new Arrow{ Colour = "0000FF", Type = ArrowType.Dotted })
            );

            await AssertDiagram(diagram, Expectations.ArrowColour);
        }

        [Fact]
        public async void MessageSequenceNumbering()
        {

            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Numbering.Start(),
                Message.Between(bob, alice, "Authentication Request"),
                Message.Between(alice, bob, "Authentication Response")
            );

            await AssertDiagram(diagram, Expectations.MessageSequenceNumbering);
        }

        [Fact]
        public async void MessageSequenceNumberingIncrement()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
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

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingIncrement);
        }

        [Fact]
        public async void MessageSequenceNumberingFormat()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
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

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingFormat);
        }

        [Fact]
        public async void MessageSequenceNumberingStopResume()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
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

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingStopResume);
        }

        [Fact]
        public async void SplittingDiagrams()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Message.Between(alice, bob, "message 1"),
                Message.Between(alice, bob, "message 2"),
                Separator.Page(),
                Message.Between(alice, bob, "message 3"),
                Message.Between(alice, bob, "message 4"),
                Separator.Page("A title for the\nlast page"),
                Message.Between(alice, bob, "message 5"),
                Message.Between(alice, bob, "message 6")
            );

            await AssertDiagram(diagram, Expectations.SplittingDiagrams);
        }

        [Theory]
        [InlineData("\t", Expectations.Grouping)]
        [InlineData("    ", Expectations.GroupingWithSpaces)]
        public async void Grouping(string indentMarker, string expectation)
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");
            var log = Participant.Called("Log");

            var diagram = SequenceDiagram.Of(
                Message.Between(alice, bob, "Authentication Request"),
                Group.Alt(new Message(bob, alice) { Label = "Authentication Accepted" }).Labelled("successful case")
                    .WithElse(
                        Group.Of(
                            Message.Between(bob, alice, "Authentication Failure"),
                            Group.Of(
                                Message.Between(alice, log, "Log attack start"),
                                Group.Loop(Message.Between(alice, bob, "DNS Attack"))
                                    .Labelled("1000 times"),
                                Message.Between(alice, log, "Log attack end")
                            ).Labelled("My own label")
                        ).Labelled("some kind of failure"),
                        Group.Of(Message.Between(bob, alice, "Please repeat"))
                            .Labelled("Another type of failure")
                )
            );

            await AssertDiagram(diagram, expectation, indentMarker);
        }

        [Fact]
        public async void Divider()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Separator.Divider("Initialization"),
                Message.Between(alice, bob, "Authentication Request"),
                Message.Between(bob, alice, "Authentication Response").WithArrow(Arrow.Dotted),
                Separator.Divider("Repetition"),
                Message.Between(alice, bob, "Another authentication Request"),
                Message.Between(bob, alice, "Another authentication Response").WithArrow(Arrow.Dotted)
            );

            await AssertDiagram(diagram, Expectations.Divider);
        }

        [Fact]
        public async void RefOver()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Actor("Bob");

            var diagram = SequenceDiagram.Of(
                alice,bob,
                Ref.Over("init", alice, bob),
                Message.Between(alice, bob, "hello"),
                Ref.Over("This can be on\nseveral lines", bob)
            );

            await AssertDiagram(diagram, Expectations.Ref);
        }

        [Fact]
        public async void DelayTest()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Message.Between(alice, bob, "Authentication Request"),
                Separator.Delay(),
                Message.Between(bob, alice, "Authentication Response").WithArrow(Arrow.Dotted),
                Separator.Delay("5 minutes later"),
                Message.Between(bob, alice, "Bye !").WithArrow(Arrow.Dotted)
            );

            await AssertDiagram(diagram, Expectations.Delay);
        }

        [Fact]
        public async void Space()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = SequenceDiagram.Of(
                Message.Between(alice, bob, "message 1"),
                Message.Between(bob, alice, "ok").WithArrow(Arrow.Dotted),
                Separator.Space(),
                Message.Between(alice, bob, "message 2"),
                Message.Between(bob, alice, "ok").WithArrow(Arrow.Dotted),
                Separator.Space(45),
                Message.Between(alice, bob, "message 3"),
                Message.Between(bob, alice, "ok").WithArrow(Arrow.Dotted)
            );

            await AssertDiagram(diagram, Expectations.Space);
        }

        [Fact]
        public async void Activation()
        {
            var user = Participant.Called("User");
            var a = Participant.Called("A");
            var b = Participant.Called("B");
            var c = Participant.Called("C");


            var diagram = SequenceDiagram.Of(
                user,
                Message.Between(user, a, "DoWork"),
                ActivationStatus.Activate(a),
                Message.Between(a, b, "<< createRequest >>"),
                ActivationStatus.Activate(b),
                Message.Between(b, c, "DoWork"),
                ActivationStatus.Activate(c),
                Message.Between(c, b, "WorkDone").WithArrow(Arrow.Dotted),
                ActivationStatus.Destroy(c),
                Message.Between(b, a, "RequestCreated").WithArrow(Arrow.Dotted),
                ActivationStatus.Deactivate(b),
                Message.Between(a, user, "Done"),
                ActivationStatus.Deactivate(a)
            );

            await AssertDiagram(diagram, Expectations.Activation);
        }

        private async Task AssertDiagram(SequenceDiagram diagram, string expectation, string indentMarker = "\t")
        {
            string result = null;

            using (var stream = new MemoryStream())
            {
                var options = new StreamRendererOptions { IndentMarker = indentMarker };
                var renderer = new StreamRenderer(stream, options);

                await diagram.Render(renderer);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    result = await reader.ReadToEndAsync();
                }
            }


            _output.WriteLine("=== EXPECTED ===");
            _output.WriteLine(expectation);
            _output.WriteLine("=== ACTUAL ===");
            _output.WriteLine(result);
            Assert.Equal(expectation, result);
        }
    }
}
