using KPlant.Rendering;
using KPlant.Tests.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class RefTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Ref();

            Assert.Null(sut.Label);
            Assert.Empty(sut.Over);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Ref();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_NullParticipants_Throws()
        {
            var sut = new Ref { Over = null };
            var renderer = new StringRenderer();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("Over", ex.Member);
            Assert.Equal(typeof(Ref), ex.Type);
        }

        [Fact]
        public async void Render_EmptyParticipants_Throws()
        {
            var sut = new Ref { Over = new List<Participant>() };
            var renderer = new StringRenderer();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("Over", ex.Member);
            Assert.Equal(typeof(Ref), ex.Type);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("  \t\t", "")]
        [InlineData("My Label", " : My Label")]
        [InlineData("My long\r\nLabel", " : My long\\nLabel")]
        [InlineData("My long\nLabel", " : My long\\nLabel")]
        public async void Render_Renders(string label, string expected)
        {
            var participant = new Participant { Id = "Alice" };
            var sut = new Ref { Over = { participant }, Label = label };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal($"ref over Alice{expected}\r\n", renderer.Value);
        }
    }
}
