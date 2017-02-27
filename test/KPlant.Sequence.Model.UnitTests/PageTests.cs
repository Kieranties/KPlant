using KPlant.Tests.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class PageTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Page();
            Assert.Null(sut.Title);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Page();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_Defaults_Renders()
        {
            var sut = new Page();
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("newpage\r\n", renderer.Value);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("My Title", " My Title")]
        [InlineData("My Long\nTitle", " My Long\\nTitle")]
        [InlineData("My Long\r\nTitle", " My Long\\nTitle")]
        public async void Render_WithTitle_Renders(string title, string expected)
        {
            var sut = new Page { Title = title };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal($"newpage{expected}\r\n", renderer.Value);
        }

    }
}
