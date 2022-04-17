using System.Globalization;
using WinFormMarkup.Extensions;

namespace BasicApp;

public class MainWindow: Form
{
    private readonly ToolStripStatusLabel _statusText;
    private readonly ToolStripStatusLabel _secondTextLabel;
    private int _counter = 0;

    public MainWindow()
    {
        this.Text("Main Window")
            .Icon(Properties.Resources.AppIcon)
            .MinimumSize(800,600)
            .StartPosition(FormStartPosition.CenterScreen)
            .MainMenuStrip(
                new ToolStripMenuItem("&File")
                    .DropDownItems(
                        new ToolStripMenuItem("&New")
                            .Keys(Keys.Control | Keys.N)
                            .Clicked(_ => CreateFile()),
                        new ToolStripMenuItem("&Open")
                            .Keys(Keys.Control | Keys.O)
                            .Clicked(_=> OpenFile()),
                        new ToolStripMenuItem("&Save")
                            .Keys(Keys.Control | Keys.S)
                            .Clicked(_=> SaveFile()),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("E&xit")
                            .Keys(Keys.Alt | Keys.F4)
                            .Clicked(_ => Application.Exit())
                    ))
            .StatusStrip(
                _statusText = new ToolStripStatusLabel("Ready")
                    .Alignment(ToolStripItemAlignment.Right)
                    .Also(label=>
                    {
                        label.TextChanged += (s, e) => _secondTextLabel.Text($"Total Changes {++_counter}");
                        label.Padding = new Padding(2, 0, 6, 0);
                        label.Clicked(_ => label.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture));
                    }),
                _secondTextLabel = new ToolStripStatusLabel()
            )
            .Controls(
                new SplitContainer()
                    .Dock(DockStyle.Fill)
                    .Panel1(
                        new TreeView()
                            .Dock(DockStyle.Fill)
                            .Nodes(
                                new TreeNode("Node 1")
                                    .Nodes(
                                        new TreeNode("Node 1.1")
                                            .Nodes(
                                                new TreeNode("Node 1.1.1")
                                            ),
                                        new TreeNode("Node 1.2")
                                    ),
                                new TreeNode("Node 2")
                                    .Nodes(
                                        new TreeNode("Node 2.1")
                                    )
                            )
                            .SetAfterSelect(node => _statusText.Text($"Selected: {node.Text}"))
                    )
                    .Panel2(
                        new TextBox()
                            .Multiline(true)
                            .Dock(DockStyle.Fill)
                            .Text("TextBox")
                            .SetTextChanged( tb => _statusText.Text($"TextChanged: Length = {tb.Text.Length}"))
                    )
                    .SplitterDistance(50)
                )
            .Show();
    }

    private void SaveFile()
    {
        _statusText.Text("Saving");
    }

    private void CreateFile()
    {
        _statusText.Text("Creating");
    }

    private void OpenFile()
    {
        _statusText.Text("opening");
    }
}