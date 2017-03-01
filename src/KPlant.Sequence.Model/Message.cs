using KPlant.Model;
using KPlant.Rendering;
using System;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    public partial class Message : ISequenceElement, IEditableLabel
    {
        public Message(Participant from, Participant to, string label = null)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Label = label;
        }

        public Arrow Arrow { get; set; } = Arrow.Default;

        public Participant From { get; }

        public string Label { get; set; } = null;

        public Participant To { get; }

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