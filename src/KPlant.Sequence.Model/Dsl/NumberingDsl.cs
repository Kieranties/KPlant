namespace KPlant.Sequence.Model
{
    public partial class Numbering
    {
        public static Numbering Resume(int? increment = null) => Create(null, increment, NumberCommand.Resume);

        public static Numbering Start(int? seed = null) => Create(seed, null, NumberCommand.Start);

        public static Numbering Start(int seed, int? increment = null) => Create(seed, increment, NumberCommand.Start);

        public static Numbering Stop() => Create(null, null, NumberCommand.Stop);

        private static Numbering Create(int? seed, int? increment, NumberCommand command) => new Numbering(command, seed, increment);
    }
}