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
            diagram.Elements.AddRange(new []
            {
                new Message{From = bob, To = alice, Label = "hello", Arrow = new Arrow{ Colour = "red" } },
                new Message{From = bob, To = alice, Label = "ok", Arrow = new Arrow{ Colour = "0000FF", Type = ArrowType.Dotted } },
            });

            await AssertDiagram(diagram, Expectations.ArrowColour);
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
