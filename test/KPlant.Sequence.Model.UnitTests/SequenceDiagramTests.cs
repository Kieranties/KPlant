using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class SequenceDiagramTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new SequenceDiagram();

            Assert.Empty(sut.Elements);
        }
    }
}
