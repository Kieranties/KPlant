using KPlant.Rendering;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KPlant.Tests.Core
{
    public class StringRenderer : IRenderer
    {
        private StringBuilder _sb = new StringBuilder();
        
        public void WriteLine(string value)
        {
            _sb.AppendLine(value);
        }

        public Task WriteLineAsync(string value)
        {
            WriteLine(value);
            return Task.CompletedTask;
        }

        public string Value => _sb.ToString();        
    }
}
