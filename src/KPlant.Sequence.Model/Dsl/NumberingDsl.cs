namespace KPlant.Sequence.Model
{
    public partial class Numbering
    {   
        public static Numbering Start(int? seed = null) => Create(seed, null, AutoNumberCommand.Start);
        public static Numbering Start(int seed, int? increment = null) => Create(seed, increment, AutoNumberCommand.Start);
        public static Numbering Stop() => Create(null, null, AutoNumberCommand.Stop);
        public static Numbering Resume(int? increment = null) => Create(null, increment, AutoNumberCommand.Resume);

        private static Numbering Create(int? seed, int? increment, AutoNumberCommand command) => new Numbering(command, seed, increment);        
    }
}
