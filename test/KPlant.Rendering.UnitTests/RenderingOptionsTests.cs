using System;
using System.Text;
using Xunit;

namespace KPlant.Rendering.Abstractions.UnitTests
{
    public class RenderingOptionsTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new RenderingOptions();

            Assert.Equal(Environment.NewLine, sut.LineEnding);
            Assert.Equal(Encoding.UTF8, sut.Encoding);
        }

        [Fact]
        public void Ctor_WithParams_SetsProperties()
        {
            var lineEnding = @"\end";
            var encoding = Encoding.UTF32;

            var sut = new RenderingOptions(lineEnding, encoding);

            Assert.Equal(lineEnding, sut.LineEnding);
            Assert.Equal(encoding, sut.Encoding);
        }        
    }    
}
