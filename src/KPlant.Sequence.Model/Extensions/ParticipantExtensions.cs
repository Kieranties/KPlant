﻿namespace KPlant.Sequence.Model
{
    public static class ParticipantExtensions
    {
        public static Participant WithColour(this Participant participant, string colour)
        {
            participant.Colour = colour;
            return participant;
        }
    }
}