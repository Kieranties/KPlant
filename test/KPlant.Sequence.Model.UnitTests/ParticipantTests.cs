using KPlant.Rendering;
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
            
            foreach(var p in participants)
            {
                var renderer = Substitute.For<IRenderer>();
                await p.Render(renderer);
                await renderer.Received(1).WriteLineAsync($"{p.Type} { p.Id}");
            }
        }
    }
}
