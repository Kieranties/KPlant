using KPlant.Rendering;
using KPlant.Tests.Core;
using NSubstitute;
using System;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class ArrowTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Arrow();

            Assert.Equal(ArrowType.Normal, sut.Type);
            Assert.Equal(ArrowHeadThickness.Normal, sut.Head.Thickness);
            Assert.Equal(ArrowHeadParts.Normal, sut.Head.Parts);
            Assert.Equal(ArrowHeadStatus.Normal, sut.Head.Status);
            Assert.Equal(null, sut.Colour);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Arrow();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_NullArrowHead_Throws()
        {
            var sut = new Arrow();
            sut.Head = null;
            var renderer = Substitute.For<IRenderer>();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("Head", ex.Member);
            Assert.Equal(typeof(Arrow), ex.Type);
        }

        [Fact]
        public async void Render_Default_Renders()
        {
            var sut = new Arrow();
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("->", renderer.Value);
        }

        [Fact]
        public async void Render_Dotted_Renders()
        {
            var sut = new Arrow { Type = ArrowType.Dotted };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("-->", renderer.Value);
        }

        [Theory]
        [InlineData("red")]
        [InlineData("0000FF")]
        public async void Render_WithColour_Renders(string colour)
        {
            var sut = new Arrow { Colour = colour };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal($"-[#{colour}]>", renderer.Value);
        }
    }
}
