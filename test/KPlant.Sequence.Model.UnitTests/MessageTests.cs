using KPlant.Rendering;
using KPlant.Tests.Core;
using NSubstitute;
using System;
using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class MessageTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Message();

            Assert.Null(sut.From);
            Assert.Null(sut.To);
            Assert.Null(sut.Label);
        }

        [Fact]
        public async void Render_NullParams_Throws()
        {
            var sut = new Message();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Render(null));
            Assert.Equal("renderer", ex.ParamName);
        }

        [Fact]
        public async void Render_NullFromParticipant_Throws()
        {
            var sut = new Message { To = new Participant() };
            var renderer = Substitute.For<IRenderer>();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("From", ex.Member);
            Assert.Equal(typeof(Message), ex.Type);
        }

        [Fact]
        public async void Render_NullToParticipant_Throws()
        {
            var sut = new Message { From = new Participant() };
            var renderer = Substitute.For<IRenderer>();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("To", ex.Member);
            Assert.Equal(typeof(Message), ex.Type);
        }

        [Fact]
        public async void Render_NullArrowParticipant_Throws()
        {
            var sut = new Message { From = new Participant(), To = new Participant(), Arrow = null };
            var renderer = Substitute.For<IRenderer>();

            var ex = await Assert.ThrowsAsync<MissingRenderingDataException>(() => sut.Render(renderer));
            Assert.Equal("Arrow", ex.Member);
            Assert.Equal(typeof(Message), ex.Type);
        }

        [Fact]
        public async void Render_WhenValid_Renders()
        {
            var from = new Participant { Id = "From" };
            var to = new Participant { Id = "To" };
            var sut = new Message { From = from, To = to };

            var renderer = new StringRenderer();

            await sut.Render(renderer);
            var result = renderer.Value;

            Assert.Equal("From -> To\r\n", result);
        }

        [Fact]
        public async void Render_WithLabel_Renders()
        {
            var from = new Participant { Id = "From" };
            var to = new Participant { Id = "To" };            
            var sut = new Message { From = from, To = to, Label = "Some info" };

            var renderer = new StringRenderer();

            await sut.Render(renderer);
            var result = renderer.Value;

            Assert.Equal("From -> To : Some info\r\n", result);
        }

        [Fact]
        public async void Render_WithCustomArrow_Renders()
        {
            var from = new Participant { Id = "From" };
            var to = new Participant { Id = "To" };
            var arrow = new Arrow
            {
                Type = ArrowType.Dotted,
                Head = new ArrowHead
                {
                    Parts = ArrowHeadParts.Bottom,
                    Status = ArrowHeadStatus.Fail,
                    Thickness = ArrowHeadThickness.Thin
                }
            };
            var sut = new Message { From = from, To = to, Label = "Some info", Arrow = arrow };

            var renderer = new StringRenderer();

            await sut.Render(renderer);
            var result = renderer.Value;

            Assert.Equal("From --//x To : Some info\r\n", result);
        }
    }
}
