using KPlant.Rendering;
using KPlant.Sequence.Model;
using System.Threading.Tasks;

namespace KPlant.Sequence.Rendering
{
    public class MessageRenderer : IModelRenderer<Message>
    {
        public async Task Render(Message model, IRenderer renderer)
        {
            var line = $"{model.From.Id} --> {model.To.Id}";
            if (!string.IsNullOrWhiteSpace(model.Label))
            {
                line += $" : {model.Label}";
            }

            await renderer.WriteLineAsync(line);
        }
    }
}
