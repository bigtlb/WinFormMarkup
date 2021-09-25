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
                    new MenuStrip
                        {
                            Items = 
                            {
                                new ToolStripMenuItem
                                {
                                    Text = "&File",
                                    DropDownItems =
                                    {
                                        new ToolStripMenuItem {ShortcutKeys = Keys.Control | Keys.N}
                                            .Clicked(sender =>
                                            {
                                                /* Do something here*/
                                            }),
                                        new ToolStripMenuItem{ ShortcutKeys = Keys.Control | Keys.O}
                                    }
                                }
                            }
                            
                        }
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
                                .Text("Status Label"),
                            new ToolStripStatusLabel().Bind(_dataModel, m=>m.ClickCount, c=>c.Text)),
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
                                .Clicked(_ => { _dataModel.ClickCount++; }))
                )
                .StartPosition(FormStartPosition.CenterScreen)
                .Text("Basic Test App");
        }
    }
}