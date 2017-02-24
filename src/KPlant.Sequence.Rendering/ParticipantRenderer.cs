using System.Threading.Tasks;
using KPlant.Rendering;
using KPlant.Sequence.Model;

namespace KPlant.Sequence.Rendering
{
    public class ParticipantRenderer : IModelRenderer<Participant>
    {
        public async Task Render(Participant model, IRenderer renderer)
        {
            var line = $"{model.Type} {model.Id}";
            await renderer.WriteLineAsync(line).ConfigureAwait(false);
        }
    }
}
