using KPlant.Tests.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class DividerTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Divider();

            Assert.Null(sut.Label);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Divider();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_Defaults_Renders()
        {
            var sut = new Divider();
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("====\r\n", renderer.Value);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("My Label", " My Label ")]
        [InlineData("My\r\nLong\r\nLabel", " My\\nLong\\nLabel ")]
        [InlineData(null, "")]
        [InlineData("This has some special characters (){}\"\"##", " This has some special characters (){}\"\"## ")]
        public async void Render_WithLabel_Renders(string label, string expected)
        {
            var sut = new Divider { Label = label };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal($"=={expected}==\r\n", renderer.Value);
        }
    }
}
