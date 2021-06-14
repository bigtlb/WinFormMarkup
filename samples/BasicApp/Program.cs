using System;
using System.Windows.Forms;

namespace BasicApp
{
    internal class Program
    {
        [STAThread]
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}