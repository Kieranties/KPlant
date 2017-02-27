using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class AutoNumber : ISequenceElement
    {
        public int? Start { get; set; } = null;

        public int? Increment { get; set; } = null;

        public string Format { get; set; } = null;

        public AutoNumberCommand Command { get; set; } = AutoNumberCommand.Start;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await renderer.WriteAsync("autonumber");
            if (Command != AutoNumberCommand.Start)
            {
                await renderer.WriteAsync($" {Command.ToString().ToLowerInvariant()}");
            }
            if (Start.HasValue) await renderer.WriteAsync($" {Start}");
            if (Increment.HasValue) await renderer.WriteAsync($" {Increment}");
            if (!string.IsNullOrWhiteSpace(Format)) await renderer.WriteAsync($" {Format.EnsureQuotes()}");
            await renderer.WriteLineAsync(string.Empty);
        }
    }

    public enum AutoNumberCommand
    {
        Start,
        Stop,
        Resume,
    }
}