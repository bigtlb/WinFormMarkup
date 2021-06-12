using System;
using System.Windows.Forms;

namespace UnitTests.Helpers
{
    internal class ControlTester : IDisposable
    {
        private readonly Form _form;

        public ControlTester(Control control)
        {
            _form = new Form();
            _form.Controls.Add(control);
            _form.Show();
        }

        public void Dispose()
        {
            _form.Dispose();
        }
    }
}