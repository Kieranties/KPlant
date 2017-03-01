using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public partial class Numbering : ISequenceElement
    {
        public Numbering(AutoNumberCommand command, int? seed = null, int? increment = null)
        {
            Seed = seed;
            Increment = increment;
            Command = command;
        }
                
        public int? Seed { get; }

        public int? Increment { get; }

        public string Format { get; set; } = null;

        public AutoNumberCommand Command { get; }

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            var output = "autonumber";            
            if (Command != AutoNumberCommand.Start)
            {
                output += $" {Command.ToString().ToLowerInvariant()}";
            }
            if (Seed.HasValue) output += $" {Seed}";
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