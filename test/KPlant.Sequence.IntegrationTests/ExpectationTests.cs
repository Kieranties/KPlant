using KPlant.Rendering;
using KPlant.Sequence.Model;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace KPlant.Sequence.IntegrationTests
{
    public class ExpectationTests
    {
        private readonly ITestOutputHelper _output;

        public ExpectationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void Basic()
        {

            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                new Message(alice, bob) { Label = "Authentication Request"},
                new Message(bob, alice) { Label = "Authentication Response", Arrow = Arrow.Dotted},
                new Message(alice, bob) {Label = "Another authentication Request"},
                new Message(bob,alice) { Label = "Another authentication Response", Arrow = Arrow.Dotted},
            };

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

            var diagram = new SequenceDiagram
            {
                p1,p2,p3,p4,p5,
                new Message(p1, p2){ Label = "To boundary"},
                new Message(p1, p3){ Label = "To control"},
                new Message(p1, p4){ Label = "To entity"},
                new Message(p1, p5){ Label = "To database"},
            };

            await AssertDiagram(diagram, Expectations.DeclaringParticipant);
        }

        [Fact]
        public async void ColourAndAliasing()
        {
            var bob = Participant.Actor("Bob").WithColour("red");
            var alice = Participant.Called("Alice");
            var l = Participant.Called("L", "I have a really\nlong name").WithColour("99FF99");

            var diagram = new SequenceDiagram
            {
                bob,alice,l,
                new Message(alice,bob) { Label = "Authentication Request" },
                new Message(bob,alice) { Label = "Authentication Response" },
                new Message(bob, l) { Label = "Log transaction" }
            };

            await AssertDiagram(diagram, Expectations.ColourAndAliasing);
        }

        [Fact(Skip = "Lacking support late declaration of participants")]
        public async void NonLetterParticipants()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob()");
            var @long = Participant.Called("Long", "This is very\nlong");

            var diagram = new SequenceDiagram
            {
                new Message(alice, bob) { Label = "Hello" },
                new Message(bob, @long),
                new Message(@long, bob) { Label = "ok" },
            };

            await AssertDiagram(diagram, Expectations.NonLetterParticipants);
        }

        [Fact]
        public async void MessageToSelf()
        {
            var alice = Participant.Called("Alice");
            var diagram = new SequenceDiagram
            {
                new Message(alice, alice) { Label = "This is a signal to self.\nIt also demonstrates\nmultiline \ntext" }
            };

            await AssertDiagram(diagram, Expectations.MessageToSelf);
        }

        [Fact]
        public async void ArrowStyle()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");
            var diagram = new SequenceDiagram
            {
                new Message(bob, alice) { Arrow = new Arrow{ Head = new ArrowHead{ Status = ArrowHeadStatus.Fail } } },
                new Message(bob, alice),
                new Message(bob, alice) { Arrow = new Arrow{ Head = new ArrowHead{ Thickness = ArrowHeadThickness.Thin } } },
                new Message(bob, alice) { Arrow = new Arrow{ Head = new ArrowHead{ Parts = ArrowHeadParts.Top } } },
                new Message(bob, alice) { Arrow = new Arrow{ Head = new ArrowHead{ Parts = ArrowHeadParts.Top, Thickness = ArrowHeadThickness.Thin } } },
                new Message(bob, alice) { Arrow = new Arrow{ Type = ArrowType.Dotted, Head = new ArrowHead{ Parts = ArrowHeadParts.Bottom, Thickness = ArrowHeadThickness.Thin } } },
                new Message(bob, alice) { Arrow = new Arrow{ Head = new ArrowHead{ Status = ArrowHeadStatus.Success } } },
                new Message(bob, alice) { Arrow = new Arrow{ Type = ArrowType.Dotted, Head = new ArrowHead{ Parts = ArrowHeadParts.Top, Thickness = ArrowHeadThickness.Thin, Status = ArrowHeadStatus.Success } } },
            };

            await AssertDiagram(diagram, Expectations.ArrowStyle);
        }

        [Fact]
        public async void ArrowColour()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                new Message(bob, alice) { Label = "hello", Arrow = new Arrow{ Colour = "red" } },
                new Message(alice, bob) { Label = "ok", Arrow = new Arrow{ Colour = "0000FF", Type = ArrowType.Dotted } },
            };

            await AssertDiagram(diagram, Expectations.ArrowColour);
        }

        [Fact]
        public async void MessageSequenceNumbering()
        {

            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                AutoNumber.Start(),
                new Message(bob, alice) { Label = "Authentication Request" },
                new Message(alice, bob) { Label = "Authentication Response" },
            };

            await AssertDiagram(diagram, Expectations.MessageSequenceNumbering);
        }

        [Fact]
        public async void MessageSequenceNumberingIncrement()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                AutoNumber.Start(),
                new Message(bob, alice) { Label = "Authentication Request" },
                new Message(alice, bob) { Label = "Authentication Response" },
                AutoNumber.Start(15),
                new Message(bob, alice) { Label = "Another authentication Request" },
                new Message(alice, bob) { Label = "Another authentication Response" },
                AutoNumber.Start(40, 10),
                new Message(bob, alice) { Label = "Yet another authentication Request" },
                new Message(alice, bob) { Label = "Yet another authentication Response" },
            };

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingIncrement);
        }

        [Fact]
        public async void MessageSequenceNumberingFormat()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                AutoNumber.Start(format: "<b>[000]"),
                new Message(bob, alice) { Label = "Authentication Request" },
                new Message(alice, bob) { Label = "Authentication Response" },
                AutoNumber.Start(15, format: "<b>(<u>##</u>)"),
                new Message(bob, alice) { Label = "Another authentication Request" },
                new Message(alice, bob) { Label = "Another authentication Response" },
                AutoNumber.Start(40, 10, "<font color=red><b>Message 0  "),
                new Message(bob, alice) { Label = "Yet another authentication Request" },
                new Message (alice, bob) { Label = "Yet another authentication Response" },
            };

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingFormat);
        }

        [Fact]
        public async void MessageSequenceNumberingStopResume()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                AutoNumber.Start(10, 10, "<b>[000]"),
                new Message(bob, alice) { Label = "Authentication Request" },
                new Message(alice, bob) { Label = "Authentication Response" },
                AutoNumber.Stop(),
                new Message(bob, alice) { Label = "dummy"},
                AutoNumber.Resume(format: "<font color=red><b>Message 0  "),
                new Message(bob, alice) { Label = "Yet another authentication Request" },
                new Message(alice, bob) { Label = "Yet another authentication Response" },
                AutoNumber.Stop(),
                new Message(bob, alice) { Label = "dummy"},
                AutoNumber.Resume(1, "<font color=blue><b>Message 0  "),
                new Message(bob, alice) { Label = "Yet another authentication Request" },
                new Message(alice, bob) { Label = "Yet another authentication Response" },
            };

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingStopResume);
        }

        [Fact]
        public async void SplittingDiagrams()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                new Message(alice, bob) { Label = "message 1"},
                new Message (alice, bob) { Label = "message 2"},
                new Page(),
                new Message(alice, bob) { Label = "message 3"},
                new Message(alice, bob) { Label = "message 4"},
                new Page("A title for the\nlast page"),
                new Message(alice, bob) { Label = "message 5"},
                new Message(alice, bob) { Label = "message 6"},
            };

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
            
            var diagram = new SequenceDiagram
            {
                new Message(alice, bob) { Label = "Authentication Request" },
                Group.Alt(new Message(bob, alice) { Label = "Authentication Accepted" }).WithLabel("successful case")
                    .WithElse(
                        Group.For(
                            new Message(bob, alice) { Label = "Authentication Failure" },
                            Group.For(
                                new Message(alice, log) { Label = "Log attack start" },
                                Group.Loop(new Message(alice, bob) { Label = "DNS Attack" })
                                    .WithLabel("1000 times"),
                                new Message(alice, log) { Label = "Log attack end" }
                            ).WithLabel("My own label")
                        ).WithLabel("some kind of failure"),
                        Group.For(new Message(bob, alice) { Label = "Please repeat" })
                            .WithLabel("Another type of failure")
                )
            };

            await AssertDiagram(diagram, expectation, indentMarker);
        }

        [Fact]
        public async void Divider()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                new Divider("Initialization"),
                new Message(alice, bob) { Label = "Authentication Request"},
                new Message(bob, alice) { Label = "Authentication Response", Arrow = Arrow.Dotted },
                new Divider("Repetition"),
                new Message(alice, bob) { Label = "Another authentication Request"},
                new Message(bob, alice) { Label = "Another authentication Response", Arrow = Arrow.Dotted },
            };

            await AssertDiagram(diagram, Expectations.Divider);
        }

        [Fact]
        public async void RefOver()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Actor("Bob");

            var diagram = new SequenceDiagram
            {
                alice,bob,
                Ref.Over("init", alice, bob),
                new Message(alice, bob) {  Label = "hello" },
                Ref.Over("This can be on\nseveral lines", bob)
            };

            await AssertDiagram(diagram, Expectations.Ref);
        }

        [Fact]
        public async void Delay()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                new Message(alice, bob) { Label = "Authentication Request"},
                new Delay(),
                new Message(bob, alice) { Label = "Authentication Response", Arrow = Arrow.Dotted},
                new Delay("5 minutes later"),
                new Message(bob, alice) { Label = "Bye !", Arrow = Arrow.Dotted},
            };

            await AssertDiagram(diagram, Expectations.Delay);
        }

        [Fact]
        public async void Space()
        {
            var alice = Participant.Called("Alice");
            var bob = Participant.Called("Bob");

            var diagram = new SequenceDiagram
            {
                new Message(alice, bob) { Label = "message 1"},
                new Message(bob, alice) { Label = "ok", Arrow = Arrow.Dotted },
                new Space(),
                new Message(alice, bob) { Label = "message 2"},
                new Message(bob, alice) { Label = "ok", Arrow = Arrow.Dotted },
                new Space { Height = 45 },
                new Message(alice, bob) { Label = "message 3"},
                new Message(bob, alice) { Label = "ok", Arrow = Arrow.Dotted },
            };

            await AssertDiagram(diagram, Expectations.Space);
        }

        [Fact]
        public async void Activation()
        {
            var user = Participant.Called("User");
            var a = Participant.Called("A");
            var b = Participant.Called("B");
            var c = Participant.Called("C");


            var diagram = new SequenceDiagram
            {
                user,
                new Message(user, a) { Label = "DoWork"},
                ActivationStatus.Activate(a),
                new Message(a, b) { Label = "<< createRequest >>"},
                ActivationStatus.Activate(b),
                new Message(b, c) { Label = "DoWork"},
                ActivationStatus.Activate(c),
                new Message(c, b) { Label = "WorkDone", Arrow = Arrow.Dotted },
                ActivationStatus.Destroy(c),
                new Message(b, a) { Label = "RequestCreated", Arrow = Arrow.Dotted },
                ActivationStatus.Deactivate(b),
                new Message(a, user) { Label = "Done" },
                ActivationStatus.Deactivate(a),
            };

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
