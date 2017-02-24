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
    }
}
