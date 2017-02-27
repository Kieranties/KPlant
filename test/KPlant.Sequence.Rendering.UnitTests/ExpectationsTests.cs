using KPlant.Sequence.Model;
using KPlant.Rendering;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.IO;


namespace KPlant.Sequence.Rendering.UnitTests
{
    public class ExpectationsTests
    {
        private readonly ITestOutputHelper _output;

        public ExpectationsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Basic()
        {
            var alice = new Participant { Id = "Alice" };
            var bob = new Participant { Id = "Bob" };

            var message1 = new Message { From = alice, To = bob, Label = "Hello!" };
            var message2 = new Message { From = bob, To = alice, Label = "World!" };

            var foo1 = new Message { From = alice, To = bob, Label = "Foo!" };
            var foo2 = new Message { From = bob, To = alice, Label = "Foo!" };

            var bar1 = new Message { From = alice, To = bob, Label = "Bar!" };
            var bar2 = new Message { From = bob, To = alice, Label = "Bar!" };

            var group = new Group
            {
                Type = GroupType.Alt,
                Label = "Foo"
            };
            group.Elements.AddRange(new[] { foo1, foo2 });
            var @else = new Group
            {
                Type = GroupType.Par,
                Label = "Bar"
            };
            @else.Elements.AddRange(new[] { bar1, bar2 });

            group.Else.Add(@else);


            var diagram = new SequenceDiagram();
            diagram.Elements.AddRange(new ISequenceElement[]
            {
                alice,bob,
                message1,message2,
                group
            });

            var options = new RenderingOptions();
            options.MapRenderer<SequenceDiagram, SequenceDiagramRenderer>();
            options.MapRenderer<Participant, ParticipantRenderer>();
            options.MapRenderer<Message, MessageRenderer>();
            options.MapRenderer<Group, GroupRenderer>();

            using (var memStream = new MemoryStream())
            using (var reader = new StreamReader(memStream))
            {
                var renderer = new Renderer(memStream, options);
                await renderer.Render(diagram);

                memStream.Seek(0, SeekOrigin.Begin);

                _output.WriteLine(reader.ReadToEnd());
            }            
        }
    }
}
