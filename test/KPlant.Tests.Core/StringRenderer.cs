using KPlant.Rendering;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KPlant.Tests.Core
{
    public class StringRenderer : IRenderer
    {
        private StringBuilder _sb = new StringBuilder();

        public void Write(string value)
        {
            _sb.Append(value);
        }

        public Task WriteAsync(string value)
        {
            Write(value);
            return Task.CompletedTask;
        }

        public void WriteLine(string value)
        {
            Write($"{value}{Environment.NewLine}");
        }

        public Task WriteLineAsync(string value)
        {
            WriteLine(value);
            return Task.CompletedTask;
        }

        public string Value => _sb.ToString();        
    }
}
