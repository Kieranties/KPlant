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

            var diagram = new SequenceDiagram();
            diagram.Elements.Add(alice);
            diagram.Elements.Add(bob);

            var memStream = new MemoryStream();
            var options = new RenderingOptions();
            options.MapRenderer<SequenceDiagram, SequenceDiagramRenderer>();
            options.MapRenderer<Participant, ParticipantRenderer>();
            var renderer = new Renderer(memStream, options);
            await renderer.Render(diagram);

            memStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memStream);
            _output.WriteLine(reader.ReadToEnd());
        }
    }
}
