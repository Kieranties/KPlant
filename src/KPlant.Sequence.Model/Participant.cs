using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Participant : ISequenceElement
    {   
        private static Participant Create(string id, ParticipantType type, string label)
        {
            return new Participant(id, label)
            {
                Type = type
            };
        }

        public static Participant Called(string id, string label = null) => Create(id, ParticipantType.Participant, label);
        public static Participant Actor(string id, string label = null) => Create(id, ParticipantType.Actor, label);
        public static Participant Boundary(string id, string label = null) => Create(id, ParticipantType.Boundary, label);
        public static Participant Control(string id, string label = null) => Create(id, ParticipantType.Control, label);
        public static Participant Entity(string id, string label = null) => Create(id, ParticipantType.Entity, label);
        public static Participant Database(string id, string label = null) => Create(id, ParticipantType.Database, label);


        public Participant(string id, string label = null)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id), "Cannot be null or whitespace");

            Id = id;
            Label = label;
        }

        public string Id { get; }

        public ParticipantType Type { get; set; } = ParticipantType.Participant;

        public string Label { get; set; }

        public string Colour { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));
            
            var output = Type.ToString().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(Label))
            {                
                output += $" {Label.FixNewlinesForOutput().EnsureQuotes()} as {Id}";
            }
            else
            {
                output += $" {Id}";
            }

            if (!string.IsNullOrWhiteSpace(Colour))
            {
                output += $" #{Colour}";
            }
            
            await renderer.WriteLineAsync(output);
        }
    }

    public enum ParticipantType
    {
        Participant,
        Actor,
        Boundary,
        Control,
        Entity,
        Database
    }

    public static class ParticipantExtensions
    {
        public static Participant WithColour(this Participant participant, string colour)
        {
            participant.Colour = colour;
            return participant;
        }
    }
}