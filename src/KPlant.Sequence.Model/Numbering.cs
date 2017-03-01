using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public partial class Numbering : ISequenceElement
    {
        public Numbering(NumberCommand command, int? seed = null, int? increment = null)
        {
            Seed = seed;
            Increment = increment;
            Command = command;
        }

        public NumberCommand Command { get; }

        public string Format { get; set; } = null;

        public int? Increment { get; }

        public int? Seed { get; }

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            var output = "autonumber";
            if (Command != NumberCommand.Start)
            {
                output += $" {Command.ToString().ToLowerInvariant()}";
            }
            if (Seed.HasValue) output += $" {Seed}";
            if (Increment.HasValue) output += $" {Increment}";
            if (!string.IsNullOrWhiteSpace(Format)) output += $" {Format.EnsureQuotes()}";

            await renderer.WriteLineAsync(output);
        }
    }

    public enum NumberCommand
    {
        Start,
        Stop,
        Resume,
    }
}