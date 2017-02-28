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

            Assert.Equal("group\r\nend\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithLabel_Renders()
        {
            var sut = new Group { Label = "My Label" };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("group My Label\r\nend\r\n", renderer.Value);
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
                Assert.Equal($"{g.Type.ToString().ToLowerInvariant()} {g.Label}\r\nend\r\n", renderer.Value);
            }
        }

        [Fact]
        public async void Render_WithElements_Renders()
        {
            var sut = new Group
            {
                new Participant{ Id = "Alpha"},
                new Participant{ Id = "Beta"},
                new Participant{ Id = "Gamma"}
            };
            var renderer = new StringRenderer();
            await sut.Render(renderer);

            Assert.Equal("group\r\nparticipant Alpha\r\nparticipant Beta\r\nparticipant Gamma\r\nend\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithElse_Renders()
        {
            var sut = new Group
            {
                Else =
                {
                    new Group(),
                    new Group
                    {
                        Label = "alt",
                        Elements = { new Participant { Type = ParticipantType.Control, Id = "other" } }
                    }
                }
            };

            var renderer = new StringRenderer();
            await sut.Render(renderer);

            Assert.Equal("group\r\nelse\r\nelse alt\r\ncontrol other\r\nend\r\n", renderer.Value);
        }
    }
}
