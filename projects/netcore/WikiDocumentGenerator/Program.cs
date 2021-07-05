using System;
using WikiDocumentGenerator.XMLToMD;

namespace WikiDocumentGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var gen = new Generator(
                @"..\..\..\..\WinFormMarkup\bin\Debug\net5.0-windows\WinFormMarkup.dll",
                @"..\..\..\..\WinFormMarkup\bin\Debug\net5.0-windows\WinFormMarkup.xml",
                "Docs",
                new string[] {"WinFormMarkup.Extensions"}
                ))
            {
                gen.Generate();
            }
        }
    }
}