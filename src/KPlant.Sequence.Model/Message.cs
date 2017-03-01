using System.Threading.Tasks;
using KPlant.Rendering;
using System;

namespace KPlant.Sequence.Model
{
    public class Message : ISequenceElement
    {
        public Message(Participant from, Participant to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
        }

        public Participant From { get; }

        public Participant To { get; }

        public string Label { get; set; } = null;

        public Arrow Arrow { get; set; } = Arrow.Default;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));
            
            if (Arrow == null)
                throw new MissingRenderingDataException(nameof(Arrow), typeof(Message));

            var output = $"{From.Id} {Arrow.Render()}";            
            var end = $" {To.Id}";
            if (!string.IsNullOrWhiteSpace(Label))
            {
                end += $" : {Label.FixNewlinesForOutput()}";
            }

            output += end;

            await renderer.WriteLineAsync(output);
        }
    }
}