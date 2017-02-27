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
