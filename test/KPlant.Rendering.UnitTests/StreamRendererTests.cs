using System;
using System.IO;
using Xunit;

namespace KPlant.Rendering.UnitTests
{
    public class StreamRendererTests
    {
        [Fact]
        public void Ctor_NullStream_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new StreamRenderer(null));

            Assert.Equal("stream", ex.ParamName);
        }
             

        [Theory]
        [InlineData("")]        
        [InlineData(null)]
        public void WriteLine_NullOrEmpty_WritesLine(string input)
        {
            var stream = new MemoryStream();
            var sut = new StreamRenderer(stream);

            sut.WriteLine(input);

            Assert.Equal("\r\n", Read(stream));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void WriteLineAsync_NullOrEmpty_WritesLine(string input)
        {
            var stream = new MemoryStream();
            var sut = new StreamRenderer(stream);

            await sut.WriteLineAsync(input);

            Assert.Equal("\r\n", Read(stream));
        }

        [Theory]
        [InlineData("   \t\t")]
        [InlineData("some value")]
        public void WriteLine_WithValue_WritesLine(string input)
        {
            var stream = new MemoryStream();
            var sut = new StreamRenderer(stream);

            sut.WriteLine(input);

            Assert.Equal($"{input}\r\n", Read(stream));
        }

        [Theory]
        [InlineData("   \t\t")]
        [InlineData("some value")]
        public async void WriteLineAsync_WithValue_WritesLine(string input)
        {
            var stream = new MemoryStream();
            var sut = new StreamRenderer(stream);

            await sut.WriteLineAsync(input);

            Assert.Equal($"{input}\r\n", Read(stream));
        }

        private string Read(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using(var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
