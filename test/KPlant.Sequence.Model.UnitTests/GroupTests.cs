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
    }
}
