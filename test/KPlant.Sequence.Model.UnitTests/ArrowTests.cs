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
        public void Render_NullArrowHead_Throws()
        {
            var sut = new Arrow();
            sut.Head = null;
            var renderer = Substitute.For<IRenderer>();

            var ex = Assert.Throws<MissingRenderingDataException>(() => sut.Render());
            Assert.Equal("Head", ex.Member);
            Assert.Equal(typeof(Arrow), ex.Type);
        }

        [Fact]
        public void Render_Default_Renders()
        {
            var sut = new Arrow();

            var result = sut.Render();

            Assert.Equal("->", result);
        }

        [Fact]
        public void Render_Dotted_Renders()
        {
            var sut = new Arrow { Type = ArrowType.Dotted };

            var result = sut.Render();

            Assert.Equal("-->", result);
        }

        [Theory]
        [InlineData("red")]
        [InlineData("0000FF")]
        public void Render_WithColour_Renders(string colour)
        {
            var sut = new Arrow { Colour = colour };

            var result = sut.Render();

            Assert.Equal($"-[#{colour}]>", result);
        }
    }
}
