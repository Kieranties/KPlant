using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class AutoNumber : ISequenceElement
    {
        private static AutoNumber Create(int? seed, int? increment, string format, AutoNumberCommand command)
        {
            return new AutoNumber { Seed = seed, Increment = increment, Format = format, Command = command };
        }

        // static resolvers
        public static AutoNumber Start(int? seed = null, int? increment = null, string format = null) => Create(seed, increment, format, AutoNumberCommand.Start);
        public static AutoNumber Stop() => Create(null, null, null, AutoNumberCommand.Stop);
        public static AutoNumber Resume(int? increment = null, string format = null) => Create(null, increment, format, AutoNumberCommand.Resume);


        public int? Seed { get; set; } = null;

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