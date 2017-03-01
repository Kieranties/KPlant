using KPlant.Rendering;
using KPlant.Sequence.Model;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace KPlant.Sequence.IntegrationTests
{
    public abstract class IntegrationTests
    {
        protected IntegrationTests(ITestOutputHelper output) => Output = output;

        protected ITestOutputHelper Output { get; }
        
        [Theory]
        [InlineData(Expectations.Basic)]
        public async void Basic(string expected) => await AssertDiagram(BasicTest(), expected);
        protected abstract SequenceDiagram BasicTest();

        [Theory]
        [InlineData(Expectations.DeclaringParticipant)]
        public async void DeclaringParticipant(string expected) => await AssertDiagram(DeclaringParticipantTest(), expected);
        protected abstract SequenceDiagram DeclaringParticipantTest();

        [Theory]
        [InlineData(Expectations.ColourAndAliasing)]
        public async void ColourAndAliasing(string expected) => await AssertDiagram(ColourAndAliasingTest(), expected);
        protected abstract SequenceDiagram ColourAndAliasingTest();
        
        [Theory(Skip = "Lacking support late declaration of participants")]
        [InlineData(Expectations.NonLetterParticipants)]
        public async void NonLetterParticipants(string expected) => await AssertDiagram(NonLetterParticipantsTest(), expected);
        protected abstract SequenceDiagram NonLetterParticipantsTest();

        [Theory]
        [InlineData(Expectations.MessageToSelf)]
        public async void MessageToSelf(string expected) => await AssertDiagram(MessageToSelfTest(), expected);
        protected abstract SequenceDiagram MessageToSelfTest();

        [Theory]
        [InlineData(Expectations.ArrowStyle)]
        public async void ArrowStyle(string expected) => await AssertDiagram(ArrowStyleTest(), expected);
        protected abstract SequenceDiagram ArrowStyleTest();

        [Theory]
        [InlineData(Expectations.ArrowColour)]
        public async void ArrowColour(string expected) => await AssertDiagram(ArrowColourTest(), expected);
        protected abstract SequenceDiagram ArrowColourTest();

        [Theory]
        
        [InlineData(Expectations.MessageSequenceNumbering)]
        public async void MessageSequenceNumbering(string expected) => await AssertDiagram(MessageSequenceNumberingTest(), expected);
        protected abstract SequenceDiagram MessageSequenceNumberingTest();

        [Theory]
        [InlineData(Expectations.MessageSequenceNumberingIncrement)]
        public async void MessageSequenceNumberingIncrement(string expected) => await AssertDiagram(MessageSequenceNumberingIncrementTest(), expected);
        protected abstract SequenceDiagram MessageSequenceNumberingIncrementTest();

        [Theory]
        [InlineData(Expectations.MessageSequenceNumberingFormat)]
        public async void MessageSequenceNumberingFormat(string expected) => await AssertDiagram(MessageSequenceNumberingFormatTest(), expected);
        protected abstract SequenceDiagram MessageSequenceNumberingFormatTest();

        [Theory]
        [InlineData(Expectations.MessageSequenceNumberingStopResume)]
        public async void MessageSequenceNumberingStopResume(string expected) => await AssertDiagram(MessageSequenceNumberingStopResumeTest(), expected);
        protected abstract SequenceDiagram MessageSequenceNumberingStopResumeTest();

        [Theory]
        [InlineData(Expectations.SplittingDiagrams)]
        public async void SplittingDiagrams(string expected) => await AssertDiagram(SplittingDiagramsTest(), expected);
        protected abstract SequenceDiagram SplittingDiagramsTest();

        [Theory]
        [InlineData(Expectations.Grouping)]
        public async void Grouping(string expected) => await AssertDiagram(GroupingTest(), expected);
        protected abstract SequenceDiagram GroupingTest();

        [Theory]
        [InlineData(Expectations.Divider)]
        public async void Divider(string expected) => await AssertDiagram(DividerTest(), expected);
        protected abstract SequenceDiagram DividerTest();

        [Theory]
        [InlineData(Expectations.Ref)]
        public async void RefBlock(string expected) => await AssertDiagram(RefBlockTest(), expected);
        protected abstract SequenceDiagram RefBlockTest();

        [Theory]
        [InlineData(Expectations.Delay)]
        public async void Delay(string expected) => await AssertDiagram(DelayTest(), expected);
        protected abstract SequenceDiagram DelayTest();

        [Theory]
        [InlineData(Expectations.Space)]
        public async void Space(string expected) => await AssertDiagram(SpaceTest(), expected);
        protected abstract SequenceDiagram SpaceTest();

        [Theory]
        [InlineData(Expectations.Activation)]
        public async void Activation(string expected) => await AssertDiagram(ActivationTest(), expected);
        protected abstract SequenceDiagram ActivationTest();

        protected async Task AssertDiagram(SequenceDiagram diagram, string expectation)
        {
            string result = null;

            using (var stream = new MemoryStream())
            {
                var options = new StreamRendererOptions();
                var renderer = new StreamRenderer(stream, options);

                await diagram.Render(renderer);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    result = await reader.ReadToEndAsync();
                }
            }


            Output.WriteLine("=== EXPECTED ===");
            Output.WriteLine(expectation);
            Output.WriteLine("=== ACTUAL ===");
            Output.WriteLine(result);
            Assert.Equal(expectation, result);
        }
    }
}
