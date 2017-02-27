using KPlant.Tests.Core;
using System;
using System.Linq;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class GroupTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Group();

            Assert.Empty(sut.Elements);
            Assert.Null(sut.Label);
            Assert.Equal(GroupType.Group, sut.Type);
            Assert.Empty(sut.Else);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Group();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_WithDefaults_Renders()
        {
            var sut = new Group();
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("Group\r\nend\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithLabel_Renders()
        {
            var sut = new Group { Label = "My Label" };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("Group My Label\r\nend\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_CustomType_Renders()
        {
            var groups = Enum.GetValues(typeof(GroupType))
                                    .Cast<GroupType>()
                                    .Select(t => new Group { Label = "xxx", Type = t });

            foreach (var g in groups)
            {
                var renderer = new StringRenderer();
                await g.Render(renderer);
                Assert.Equal($"{g.Type} {g.Label}\r\nend\r\n", renderer.Value);
            }
        }

        [Fact]
        public async void Render_WithElements_Renders()
        {
            var sut = new Group();
            sut.Elements.AddRange(new[]
            {
                new Participant{ Id = "Alpha"},
                new Participant{ Id = "Beta"},
                new Participant{ Id = "Gamma"}
            });
            var renderer = new StringRenderer();
            await sut.Render(renderer);

            Assert.Equal("Group\r\nParticipant Alpha\r\nParticipant Beta\r\nParticipant Gamma\r\nend\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithElse_Renders()
        {
            var sut = new Group();
            var control = new Participant { Type = ParticipantType.Control, Id = "other" };
            var @else = new Group { Label = "alt" };
            @else.Elements.Add(control);

            sut.Else.AddRange(new[]
            {
                new Group(),
                @else
            });
            var renderer = new StringRenderer();
            await sut.Render(renderer);

            Assert.Equal("Group\r\nelse\r\nelse alt\r\nControl other\r\nend\r\n", renderer.Value);
        }
    }
}
