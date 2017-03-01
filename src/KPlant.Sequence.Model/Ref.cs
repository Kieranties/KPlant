﻿using KPlant.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    ///<remarks>Multi-line wrapped statements not supported</remarks>
    public class Ref : ISequenceElement
    {
        public Ref(string label, params Participant[] participants)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentOutOfRangeException(nameof(label));

            Label = label;
            Participants = new List<Participant>(participants);
        }

        public string Label { get; }

        public List<Participant> Participants { get; }

        public static Ref Over(string label, params Participant[] participants) => new Ref(label, participants);

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (string.IsNullOrWhiteSpace(Label))
                throw new MissingRenderingDataException(nameof(Label), typeof(Ref));

            if (Participants == null || Participants.Count == 0)
                throw new MissingRenderingDataException(nameof(Participants), typeof(Ref));

            var output = "ref over ";
            output += string.Join(", ", Participants.Select(x => x.Id));
            output += $" : {Label.FixNewlinesForOutput()}";

            await renderer.WriteLineAsync(output);
        }
    }
}