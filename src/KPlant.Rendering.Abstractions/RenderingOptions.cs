using System;
using System.Collections.Generic;
using System.Text;

namespace KPlant.Rendering
{
    public class RenderingOptions
    {
        private readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type>();

        public RenderingOptions(string lineEnding = null, Encoding encoding = null)
        {
            LineEnding = lineEnding ?? Environment.NewLine;
            Encoding = encoding ?? Encoding.UTF8;
        }

        public string LineEnding { get; }

        public Encoding Encoding { get; }

        public void MapRenderer<TModel, TModelRenderer>() where TModelRenderer : IModelRenderer<TModel>
        {
            _map[typeof(TModel)] = typeof(TModelRenderer);
        }

        public Type ResolveRenderer(Type type)
        {
            if (_map.TryGetValue(type, out var result))
            {
                return result;
            };

            return null;
        }
    }
}
