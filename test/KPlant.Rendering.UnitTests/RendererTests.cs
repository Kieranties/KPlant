using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KPlant.Rendering.UnitTests
{
    public class RendererTests
    {
        [Fact]
        public void Ctor_NullStream_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Renderer(null));

            Assert.Equal("stream", ex.ParamName);
        }

        [Fact]
        public void Ctor_NullOptions_SetsDefaults()
        {
            var stream = Substitute.For<Stream>();
            var sut = new Renderer(stream, null);

            Assert.NotNull(sut.Options);
        }

        [Fact]
        public void Ctor_WithParams_SetsProperties()
        {
            var stream = Substitute.For<Stream>();
            var options = new RenderingOptions();

            var sut = new Renderer(stream, options);

            Assert.Equal(stream, sut.Stream);
            Assert.Equal(options, sut.Options);
        }        
    }
}
