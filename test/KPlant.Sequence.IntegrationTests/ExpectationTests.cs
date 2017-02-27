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
            var diagram = new SequenceDiagram();
            var alice = new Participant { Id = "Alice" };
            var bob = new Participant { Id = "Bob" };

            diagram.Elements.AddRange(new[]
            {
                new Message{ From = alice, To = bob, Label = "Authentication Request"},
                new Message{ From = bob, To = alice, Label = "Authentication Response", Arrow = Arrow.Dotted},
                new Message{ From = alice, To = bob, Label = "Another authentication Request"},
                new Message{ From = bob, To = alice, Label = "Another authentication Response", Arrow = Arrow.Dotted},
            });

            await AssertDiagram(diagram, Expectations.Basic);
        }

        [Fact]
        public async void DeclaringParticipant()
        {
            var particpants = new[]
            {
                new Participant{ Id = "Foo1", Type = ParticipantType.Actor },
                new Participant{ Id = "Foo2", Type = ParticipantType.Boundary },
                new Participant{ Id = "Foo3", Type = ParticipantType.Control },
                new Participant{ Id = "Foo4", Type = ParticipantType.Entity },
                new Participant{ Id = "Foo5", Type = ParticipantType.Database }
            };
            var messages = new[]
            {
                new Message{ From = particpants[0], To = particpants[1], Label = "To boundary"},
                new Message{ From = particpants[0], To = particpants[2], Label = "To control"},
                new Message{ From = particpants[0], To = particpants[3], Label = "To entity"},
                new Message{ From = particpants[0], To = particpants[4], Label = "To database"},
            };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(particpants);
            diagram.Elements.AddRange(messages);

            await AssertDiagram(diagram, Expectations.DeclaringParticipant);
        }

        [Fact]
        public async void ColourAndAliasing()
        {
            var bob = new Participant { Id = "Bob", Type = ParticipantType.Actor, Colour = "red" };
            var alice = new Participant { Id = "Alice" };
            var l = new Participant { Id = "L", Label = "I have a really\nlong name", Colour = "99FF99" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                bob,alice,l,
                new Message{ From = alice, To = bob, Label = "Authentication Request" },
                new Message{ From = bob, To = alice, Label = "Authentication Response" },
                new Message{ From = bob, To = l, Label = "Log transaction" }
            });

            await AssertDiagram(diagram, Expectations.ColourAndAliasing);
        }

        [Fact(Skip = "Lacking support late declaration of participants")]
        public async void NonLetterParticipants()
        {
            var alice = new Participant { Id = "Alice" };
            var bob = new Participant { Id = "Bob()" };
            var @long = new Participant { Id = "Long", Label = "This is very\nlong" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new[]
            {
                new Message{ From = alice, To = bob, Label = "Hello"},
                new Message{ From = bob, To = @long },
                new Message{ From = @long, To = bob, Label = "ok"},
            });

            await AssertDiagram(diagram, Expectations.NonLetterParticipants);
        }

        [Fact]
        public async void MessageToSelf()
        {
            var alice = new Participant { Id = "Alice" };
            var diagram = new SequenceDiagram();
            diagram.Elements.Add(new Message { From = alice, To = alice, Label = "This is a signal to self.\nIt also demonstrates\nmultiline \ntext" });

            await AssertDiagram(diagram, Expectations.MessageToSelf);
        }

        [Fact]
        public async void ArrowStyle()
        {
            var alice = new Participant { Id = "Alice" };
            var bob = new Participant { Id = "Bob" };
            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new[]
            {
                new Message { From = bob, To = alice, Arrow = new Arrow{ Head = new ArrowHead{ Status = ArrowHeadStatus.Fail } } },
                new Message { From = bob, To = alice },
                new Message { From = bob, To = alice, Arrow = new Arrow{ Head = new ArrowHead{ Thickness = ArrowHeadThickness.Thin } } },
                new Message { From = bob, To = alice, Arrow = new Arrow{ Head = new ArrowHead{ Parts = ArrowHeadParts.Top } } },
                new Message { From = bob, To = alice, Arrow = new Arrow{ Head = new ArrowHead{ Parts = ArrowHeadParts.Top, Thickness = ArrowHeadThickness.Thin } } },
                new Message { From = bob, To = alice, Arrow = new Arrow{ Type = ArrowType.Dotted, Head = new ArrowHead{ Parts = ArrowHeadParts.Bottom, Thickness = ArrowHeadThickness.Thin } } },
                new Message { From = bob, To = alice, Arrow = new Arrow{ Head = new ArrowHead{ Status = ArrowHeadStatus.Success } } },
                new Message { From = bob, To = alice, Arrow = new Arrow{ Type = ArrowType.Dotted, Head = new ArrowHead{ Parts = ArrowHeadParts.Top, Thickness = ArrowHeadThickness.Thin, Status = ArrowHeadStatus.Success } } },
            });

            await AssertDiagram(diagram, Expectations.ArrowStyle);
        }

        [Fact]
        public async void ArrowColour()
        {
            var alice = new Participant { Id = "Alice" };
            var bob = new Participant { Id = "Bob" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new[]
            {
                new Message{From = bob, To = alice, Label = "hello", Arrow = new Arrow{ Colour = "red" } },
                new Message{From = alice, To = bob, Label = "ok", Arrow = new Arrow{ Colour = "0000FF", Type = ArrowType.Dotted } },
            });

            await AssertDiagram(diagram, Expectations.ArrowColour);
        }

        [Fact]
        public async void MessageSequenceNumbering()
        {

            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new AutoNumber(),
                new Message { From = bob, To = alice, Label = "Authentication Request" },
                new Message { From = alice, To = bob, Label = "Authentication Response" },
            });

            await AssertDiagram(diagram, Expectations.MessageSequenceNumbering);
        }

        [Fact]
        public async void MessageSequenceNumberingIncrement()
        {
            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new AutoNumber(),
                new Message { From = bob, To = alice, Label = "Authentication Request" },
                new Message { From = alice, To = bob, Label = "Authentication Response" },
                new AutoNumber { Start = 15 },
                new Message { From = bob, To = alice, Label = "Another authentication Request" },
                new Message { From = alice, To = bob, Label = "Another authentication Response" },
                new AutoNumber { Start = 40, Increment = 10 },
                new Message { From = bob, To = alice, Label = "Yet another authentication Request" },
                new Message { From = alice, To = bob, Label = "Yet another authentication Response" },
            });

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingIncrement);
        }

        [Fact]
        public async void MessageSequenceNumberingFormat()
        {
            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new AutoNumber { Format = "<b>[000]" },
                new Message { From = bob, To = alice, Label = "Authentication Request" },
                new Message { From = alice, To = bob, Label = "Authentication Response" },
                new AutoNumber { Start = 15, Format = "<b>(<u>##</u>)" },
                new Message { From = bob, To = alice, Label = "Another authentication Request" },
                new Message { From = alice, To = bob, Label = "Another authentication Response" },
                new AutoNumber { Start = 40, Increment = 10, Format = "<font color=red><b>Message 0  " },
                new Message { From = bob, To = alice, Label = "Yet another authentication Request" },
                new Message { From = alice, To = bob, Label = "Yet another authentication Response" },
            });

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingFormat);
        }

        [Fact]
        public async void MessageSequenceNumberingStopResume()
        {
            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new AutoNumber { Format = "<b>[000]", Start = 10, Increment = 10 },
                new Message { From = bob, To = alice, Label = "Authentication Request" },
                new Message { From = alice, To = bob, Label = "Authentication Response" },
                new AutoNumber { Command = AutoNumberCommand.Stop },
                new Message { From = bob, To = alice, Label = "dummy"},
                new AutoNumber { Format = "<font color=red><b>Message 0  ", Command = AutoNumberCommand.Resume },
                new Message { From = bob, To = alice, Label = "Yet another authentication Request" },
                new Message { From = alice, To = bob, Label = "Yet another authentication Response" },
                new AutoNumber { Command = AutoNumberCommand.Stop },
                new Message { From = bob, To = alice, Label = "dummy"},
                new AutoNumber { Start = 1, Format = "<font color=blue><b>Message 0  ", Command = AutoNumberCommand.Resume },
                new Message { From = bob, To = alice, Label = "Yet another authentication Request" },
                new Message { From = alice, To = bob, Label = "Yet another authentication Response" },
            });

            await AssertDiagram(diagram, Expectations.MessageSequenceNumberingStopResume);
        }

        [Fact]
        public async void SplittingDiagrams()
        {
            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new Message { From = alice, To = bob, Label = "message 1"},
                new Message { From = alice, To = bob, Label = "message 2"},
                new Page(),
                new Message { From = alice, To = bob, Label = "message 3"},
                new Message { From = alice, To = bob, Label = "message 4"},
                new Page { Title = "A title for the\nlast page"},
                new Message { From = alice, To = bob, Label = "message 5"},
                new Message { From = alice, To = bob, Label = "message 6"},
            });

            await AssertDiagram(diagram, Expectations.SplittingDiagrams);
        }

        [Fact(Skip = "Indenting currently not supported")]
        public async void Grouping()
        {
            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };
            var log = new Participant { Id = "Log" };
                        
            var loop = new Group { Type = GroupType.Loop, Label = "1000 times" };
            loop.Elements.Add(new Message { From = alice, To = bob, Label = "DNS Attack" });

            var innerGroup = new Group { Label = "My own label" };
            innerGroup.Elements.AddRange(new ISequenceElement[]
            {
                new Message { From = alice, To = log, Label = "Log attack start"},
                loop,
                new Message { From = alice, To = log, Label = "Log attack end"},
            });

            var else1 = new Group { Label = "some kind of failure" };
            else1.Elements.AddRange(new ISequenceElement[] {
                new Message { From = bob, To = alice, Label = "Authentication Failure" },
                innerGroup
            });

            var else2 = new Group { Label = "Another type of failure" };
            else2.Elements.Add(new Message { From = bob, To = alice, Label = "Please repeat" });


            var alt = new Group { Type = GroupType.Alt, Label = "successful case" };
            alt.Elements.Add(new Message { From = bob, To = alice, Label = "Authentication Accepted" });
            alt.Else.AddRange(new[] { else1, else2 });

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new Message { From = alice, To = bob, Label = "Authentication Request" },
                alt
            });

            await AssertDiagram(diagram, Expectations.Grouping);
        }

        [Fact]
        public async void Divider()
        {
            var bob = new Participant { Id = "Bob" };
            var alice = new Participant { Id = "Alice" };

            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                new Divider { Label = "Initialization" },
                new Message { From = alice, To = bob, Label = "Authentication Request"},
                new Message { From = bob, To = alice, Label = "Authentication Response", Arrow = Arrow.Dotted },
                new Divider { Label = "Repetition" },
                new Message { From = alice, To = bob, Label = "Another authentication Request"},
                new Message { From = bob, To = alice, Label = "Another authentication Response", Arrow = Arrow.Dotted },
            });
        }

        private async Task AssertDiagram(SequenceDiagram diagram, string expectation)
        {
            string result = null;

            using (var stream = new MemoryStream())
            {
                var renderer = new Renderer(stream);
                await diagram.Render(renderer);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    result = await reader.ReadToEndAsync();
                }
            }


            _output.WriteLine(result);
            Assert.Equal(expectation, result);
        }
    }
}
