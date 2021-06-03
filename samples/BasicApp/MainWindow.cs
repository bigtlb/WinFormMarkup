using System.Drawing;
using System.Windows.Forms;
using WinFormMarkup.Extensions;

namespace BasicApp
{
    public class MainWindow : Form
    {
        private readonly DataModel _dataModel = new();

        public MainWindow()
        {
            // Control instantiation
            this.Controls(
                    new MenuStrip()
                        .Items(
                            new ToolStripMenuItem("&File")
                                .DropDownItems(
                                    new ToolStripMenuItem("&New")
                                        .Keys(Keys.Control | Keys.N),
                                    new ToolStripMenuItem("&Open")
                                        .Keys(Keys.Control | Keys.O)
                                )),
                    new StatusStrip()
                        .Items(
                            new ToolStripStatusLabel()
                                .Text("Status Label")),
                    new FlowLayoutPanel()
                        .Dock(DockStyle.Fill)
                        .BackColor(Color.Aqua)
                        .Padding(0, 20)
                        .AutoScroll(true)
                        .FlowDirection(FlowDirection.TopDown)
                        .WrapContents(false)
                        .Controls(
                            new Label()
                                .AutoSize(true)
                                .Binding(_dataModel, source => source.ClickMessage),
                            new Button()
                                .Text("Click Me")
                                .Clicked(c => { _dataModel.ClickCount++; }))
                )
                .StartPosition(FormStartPosition.CenterScreen)
                .Text("Basic Test App");
        }
    }
}