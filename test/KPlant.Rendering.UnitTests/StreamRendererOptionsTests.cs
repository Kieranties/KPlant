using System;
using System.Text;
using Xunit;

namespace KPlant.Rendering.Abstractions.UnitTests
{
    public class StreamRendererOptionsTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new StreamRendererOptions();

            Assert.Equal(Environment.NewLine, sut.LineEnding);
            Assert.Equal(Encoding.UTF8, sut.Encoding);
        }     
    }    
}
