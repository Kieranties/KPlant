using Xunit;

namespace KPlant.Sequence.Model.UnitTests
{
    public class ParticipantTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new Participant();

            Assert.Null(sut.Id);
            Assert.Equal(ParticipantType.Participant, sut.Type);
        }
    }
}
