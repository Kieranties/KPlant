using System.Threading.Tasks;
using KPlant.Rendering;
using System;

namespace KPlant.Sequence.Model
{
    public class Message : ISequenceElement
    {
        public Participant From { get; set; }

        public Participant To { get; set; }

        public string Label { get; set; }

        public Arrow Arrow { get; set; } = Arrow.Default;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (From == null)
                throw new MissingRenderingDataException(nameof(From), typeof(Message));

            if (To == null)
                throw new MissingRenderingDataException(nameof(To), typeof(Message));

            if (Arrow == null)
                throw new MissingRenderingDataException(nameof(Arrow), typeof(Message));

            await renderer.WriteAsync($"{From.Id} ");
            await Arrow.Render(renderer);
            var end = $" {To.Id}";
            if (!string.IsNullOrWhiteSpace(Label))
            {
                end += $" : {Label.FixNewlinesForOutput()}";
            }

            await renderer.WriteLineAsync(end);
        }
    }
}