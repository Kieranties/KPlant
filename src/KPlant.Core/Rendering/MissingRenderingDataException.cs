using System;

namespace KPlant.Rendering
{
    public class MissingRenderingDataException : Exception
    {
        public MissingRenderingDataException(string member, Type type)
            : base($"{member} must be populated on {type} to be renderer correctly")
        {
            Member = member;
            Type = type;
        }

        public string Member { get; }

        public Type Type { get; }
    }
}
