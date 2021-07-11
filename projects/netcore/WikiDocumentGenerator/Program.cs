using WikiDocumentGenerator.XMLToMD;

namespace WikiDocumentGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var gen = new Generator(
                @"..\..\..\..\WinFormMarkup\bin\Debug\net5.0-windows\WinFormMarkup.dll",
                @"..\..\..\..\WinFormMarkup\bin\Debug\net5.0-windows\WinFormMarkup.xml",
                "Docs",
                new[] {"WinFormMarkup.Extensions"}
            ))
            {
                gen.Generate();
            }
        }
    }
}