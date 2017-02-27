using KPlant.Rendering;
using KPlant.Tests.Core;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class ParticipantTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Participant();

            Assert.Null(sut.Id);
            Assert.Equal(ParticipantType.Participant, sut.Type);
            Assert.Null(sut.Label);
            Assert.Null(sut.Colour);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Participant();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    \t\t")]
        [InlineData(null)]
        public async void Render_NullOrEmptyId_Throws(string id)
        {
            var sut = new Participant { Id = id };
            var renderer = Substitute.For<IRenderer>();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("Id", ex.Member);
            Assert.Equal(typeof(Participant), ex.Type);
        }

        [Fact]
        public async void Render_WithType_Renders()
        {
            var participants = Enum.GetValues(typeof(ParticipantType))
                                    .Cast<ParticipantType>()
                                    .Select(t => new Participant { Id = "xxx", Type = t });

            foreach (var p in participants)
            {
                var renderer = new StringRenderer();
                await p.Render(renderer);
                Assert.Equal($"{p.Type.ToString().ToLowerInvariant()} {p.Id}\r\n", renderer.Value);
            }
        }

        [Fact]
        public async void Render_WithLabel_Renders()
        {
            var sut = new Participant { Id = "MyId", Label = "This is a label" };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("participant \"This is a label\" as MyId\r\n", renderer.Value);
        }

        [Theory]
        [InlineData("\r\n")]
        [InlineData("\n")]
        public async void Render_LabelWithNewlines_Renders(string newline)
        {
            var sut = new Participant { Id = "MyId", Label = "This is a" + newline + "long label" };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            var expected = "participant \"This is a\\nlong label\" as MyId\r\n";
            var actual = renderer.Value;
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("red")]
        [InlineData("99FF99")]
        public async void Render_WithColour_Renders(string colour)
        {
            var sut = new Participant { Id = "MyId", Colour = colour };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal($"participant MyId #{colour}\r\n", renderer.Value);
        }
    }    
}
