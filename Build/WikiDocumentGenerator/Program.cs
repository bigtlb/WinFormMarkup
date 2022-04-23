using WikiDocumentGenerator.XMLToMD;

namespace WikiDocumentGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var gen = new Generator(
                @"WinFormMarkup.dll",
                @"WinFormMarkup.xml",
                "Docs",
                new[] {"WinFormMarkup.Extensions"}
            ))
            {
                gen.Generate();
            }
        }
    }
}