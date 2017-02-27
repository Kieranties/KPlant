using KPlant.Tests.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace KPlant.Sequence.Model.UnitTests
{
    public class AutoNumberTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new AutoNumber();

            Assert.Null(sut.Start);
            Assert.Null(sut.Increment);
            Assert.Null(sut.Format);
            Assert.Equal(AutoNumberCommand.Start, sut.Command);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new AutoNumber();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_Defaults_Renders()
        {
            var sut = new AutoNumber();
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("autonumber\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithFormat_Renders()
        {
            var sut = new AutoNumber { Format = "<b>(<u>##</u>)" };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("autonumber \"<b>(<u>##</u>)\"\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithStart_Renders()
        {
            var sut = new AutoNumber { Start = 10 };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("autonumber 10\r\n", renderer.Value);
        }

        [Fact]
        public async void Render_WithStartAndIncrement_Renders()
        {
            var sut = new AutoNumber { Start = 10, Increment = 20 };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal("autonumber 10 20\r\n", renderer.Value);
        }

        [Theory]
        [InlineData(AutoNumberCommand.Start, "")]
        [InlineData(AutoNumberCommand.Stop, "stop ")]
        [InlineData(AutoNumberCommand.Resume, "resume ")]
        public async void Render_WithCommand_Renders(AutoNumberCommand command, string expected)
        {
            var sut = new AutoNumber { Start = 10, Increment = 20, Command = command };
            var renderer = new StringRenderer();

            await sut.Render(renderer);

            Assert.Equal($"autonumber {expected}10 20\r\n", renderer.Value);
        }
    }
}
