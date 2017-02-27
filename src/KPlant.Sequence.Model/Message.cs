using System.Threading.Tasks;
using KPlant.Rendering;

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
            await renderer.WriteAsync($"{From.Id} ");
            await Arrow.Render(renderer);
            var end = $" {To.Id}";
            if (!string.IsNullOrWhiteSpace(Label))
            {
                end += $" : {Label}";
            }

            await renderer.WriteLineAsync(end);
        }
    }
}