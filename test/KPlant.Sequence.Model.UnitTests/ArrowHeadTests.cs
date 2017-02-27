using KPlant.Tests.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class ArrowHeadTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new ArrowHead();
            Assert.Equal(ArrowHeadThickness.Normal, sut.Thickness);
            Assert.Equal(ArrowHeadParts.Normal, sut.Parts);
            Assert.Equal(ArrowHeadStatus.Normal, sut.Status);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new ArrowHead();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_Defaults_Renders()
        {
            var sut = new ArrowHead();
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal(">", renderer.Value);
        }

        [Theory]        
        [InlineData(ArrowHeadParts.Normal, ">")]
        [InlineData(ArrowHeadParts.Top, @"\")]
        [InlineData(ArrowHeadParts.Bottom, "/")]
        public async void Render_Parts_Renders(ArrowHeadParts parts, string expected)
        {
            var sut = new ArrowHead { Parts = parts };
            var renderer = new StringRenderer();

            await sut.Render(renderer);
            Assert.Equal(expected, renderer.Value);
        }

        [Theory]
        [InlineData(ArrowHeadParts.Normal, ">>")]
        [InlineData(ArrowHeadParts.Top, @"\\")]
        [InlineData(ArrowHeadParts.Bottom, "//")]
        public async void Render_ThinParts_Renders(ArrowHeadParts parts, string expected)
        {
            var sut = new ArrowHead { Parts = parts, Thickness = ArrowHeadThickness.Thin };
            var renderer = new StringRenderer();

            await sut.Render(renderer);
            Assert.Equal(expected, renderer.Value);
        }

        [Theory]
        [InlineData(ArrowHeadStatus.Success, ">o")]
        [InlineData(ArrowHeadStatus.Fail, ">x")]
        public async void Render_Status_Renders(ArrowHeadStatus status, string expected)
        {
            var sut = new ArrowHead { Status = status };
            var renderer = new StringRenderer();

            await sut.Render(renderer);
            Assert.Equal(expected, renderer.Value);
        }
    }
}
