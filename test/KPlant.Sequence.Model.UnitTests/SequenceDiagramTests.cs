using KPlant.Rendering;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class SequenceDiagramTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new SequenceDiagram();

            Assert.Empty(sut.Elements);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new SequenceDiagram();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_NoElements_RendersEmptyDiagram()
        {
            var sut = new SequenceDiagram();
            sut.Elements.Clear(); // ensure empty
            var renderer = Substitute.For<IRenderer>();

            await sut.Render(renderer);

            await renderer.Received(1).WriteLineAsync("@startuml");
            await renderer.Received(1).WriteLineAsync("@enduml");
            Assert.Equal(2, renderer.ReceivedCalls().Count());
        }

        [Fact]
        public async void Render_WithElements_RendersElements()
        {
            var sut = new SequenceDiagram();
            var elements = Enumerable.Range(1, 3).Select(_ => Substitute.For<ISequenceElement>()).ToList();
            sut.Elements.AddRange(elements);
            var renderer = Substitute.For<IRenderer>();

            await sut.Render(renderer);

            foreach(var e in elements)
            {
                await e.Received(1).Render(renderer);
            }
        }
    }
}
