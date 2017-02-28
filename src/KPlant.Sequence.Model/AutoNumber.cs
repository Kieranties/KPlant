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

            var output = "autonumber";            
            if (Command != AutoNumberCommand.Start)
            {
                output += $" {Command.ToString().ToLowerInvariant()}";
            }
            if (Start.HasValue) output += $" {Start}";
            if (Increment.HasValue) output += $" {Increment}";
            if (!string.IsNullOrWhiteSpace(Format)) output += $" {Format.EnsureQuotes()}";

            await renderer.WriteLineAsync(output);
        }
    }

    public enum AutoNumberCommand
    {
        Start,
        Stop,
        Resume,
    }
}