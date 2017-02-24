using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KPlant.Rendering.Abstractions.UnitTests
{
    public class DiagramRendererTests
    {
        [Fact]
        public async Task Render_NullModel_ThrowsException()
        {
            var sut = new StubDiagramRenderer();
            var renderer = Substitute.For<IRenderer>();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null, renderer));
            Assert.Equal("model", ex.ParamName);
        }

        [Fact]
        public async Task Render_NullRenderer_ThrowsException()
        {
            var sut = new StubDiagramRenderer();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(new object(), null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async Task Render_WritesStartLine()
        {
            var sut = new StubDiagramRenderer();
            var model = new object();
            var renderer = Substitute.For<IRenderer>();

            await sut.Render(model, renderer);

            await renderer.Received(1).WriteLineAsync("@startuml");
        }

        [Fact]
        public async Task Render_InvokesRenderDiagram()
        {
            var sut = new StubDiagramRenderer();
            var model = new object();
            var renderer = Substitute.For<IRenderer>();

            await sut.Render(model, renderer);

            Assert.True(sut.Called);
            Assert.Equal(model, sut.Diagram);
            Assert.Equal(renderer, sut.Renderer);
        }

        [Fact]
        public async Task Render_WritesEndLine()
        {
            var sut = new StubDiagramRenderer();
            var model = new object();
            var renderer = Substitute.For<IRenderer>();

            await sut.Render(model, renderer);

            await renderer.Received(1).WriteLineAsync("@enduml");
        }
    }

    public class StubDiagramRenderer : DiagramRenderer<object>
    {   
        protected override Task RenderDiagram(object diagram, IRenderer renderer)
        {
            Called = true;
            Diagram = diagram;
            Renderer = renderer;

            return Task.CompletedTask;
        }

        public bool Called { get; protected set; }

        public object Diagram { get; protected set; }

        public IRenderer Renderer { get; protected set; }
    }
}
