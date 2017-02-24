using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;

namespace KPlant.Rendering.Abstractions.UnitTests
{
    public class RenderingOptionsTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new RenderingOptions();

            Assert.Equal(Environment.NewLine, sut.LineEnding);
            Assert.Equal(Encoding.UTF8, sut.Encoding);
        }

        [Fact]
        public void Ctor_WithParams_SetsProperties()
        {
            var lineEnding = @"\end";
            var encoding = Encoding.UTF32;

            var sut = new RenderingOptions(lineEnding, encoding);

            Assert.Equal(lineEnding, sut.LineEnding);
            Assert.Equal(encoding, sut.Encoding);
        }

        [Fact]
        public void MapRenderer_MultipleCalls_DoesNotThrow()
        {
            var sut = new RenderingOptions();
            var renderer = Substitute.For<IModelRenderer<object>>();

            sut.MapRenderer<object, IModelRenderer<object>>();
            sut.MapRenderer<object, IModelRenderer<object>>();
            sut.MapRenderer<object, IModelRenderer<object>>();
        }

        [Fact]
        public void ResolveRenderer_NotMapped_ReturnsNull()
        {
            var sut = new RenderingOptions();

            var result = sut.ResolveRenderer(typeof(object));
            Assert.Null(result);
        }

        [Fact]
        public void ResolveRenderer_WhenMapped_ReturnsType()
        {
            var sut = new RenderingOptions();

            sut.MapRenderer<object, StubRenderer<object>>();

            var result = sut.ResolveRenderer(typeof(object));
            Assert.Equal(result, typeof(StubRenderer<object>));
        }

        [Fact]
        public void ResolveRenderer_WhenReMapped_ReturnsType()
        {
            var sut = new RenderingOptions();

            sut.MapRenderer<object, StubRenderer<object>>();
            sut.MapRenderer<object, StubRenderer2<object>>();

            var result = sut.ResolveRenderer(typeof(object));
            Assert.Equal(result, typeof(StubRenderer2<object>));
        }
    }

    public class StubRenderer<T> : IModelRenderer<T>
    {
        public Task Render(T model, IRenderer renderer)
        {
            throw new NotImplementedException();
        }
    }

    public class StubRenderer2<T> : IModelRenderer<T>
    {
        public Task Render(T model, IRenderer renderer)
        {
            throw new NotImplementedException();
        }
    }
}
