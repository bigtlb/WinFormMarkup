using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using ReactiveUI;
using WinFormMarkup.Extensions;

namespace ReactiveUIApp.Views
{
    public partial class MainView
    {
        private ToolStripMenuItem _exitMenu;
        private TextBox _firstName;
        private TextBox _lastName;
        private TextBox _street;

        private void Bind()
        {
            // Control instantiation
            this.Controls(
                    new MenuStrip()
                        .Items(
                            new ToolStripMenuItem("&File")
                                .DropDownItems(
                                    _exitMenu = new ToolStripMenuItem("E&xit")
                                        .Keys(Keys.Alt | Keys.F4)
                                )),
                    new StatusStrip()
                        .Items(
                            new ToolStripStatusLabel()
                                .Text("Status Label")),
                    new TableLayoutPanel()
                        .TableLayout(2, 6)
                        .Padding(20)
                        .Dock(DockStyle.Fill)
                        .BackColor(Color.LightGray)
                        .TableControls(
                            new(new Label().Text("First Name:").TextAlign(ContentAlignment.MiddleRight), 0, 0),
                            new(_firstName = new TextBox(), 1, 0),
                            new(new Label().Text("Last Name:").TextAlign(ContentAlignment.MiddleRight), 0, 1),
                            new(_lastName = new TextBox(), 1, 1),
                            new(new Label().Text("Street:").TextAlign(ContentAlignment.MiddleRight), 0, 2),
                            new(_street = new TextBox(), 1, 2)
                        )
                )
                .AutoSize(true)
                .StartPosition(FormStartPosition.CenterScreen)
                .Text("Basic Test App");
        }

        private void Build()
        {
            this.WhenActivated(d =>
            {
                d(this.BindCommand(ViewModel, vm => vm.ExitCommand, v => v._exitMenu));
                d(this.Bind(ViewModel, vm => vm.FirstName, v => v._firstName.Text));
                d(this.Bind(ViewModel, vm => vm.LastName, v => v._lastName.Text));
                d(this.Bind(ViewModel, vm => vm.Street, v => v._street.Text));
                d(ViewModel.closeApp.RegisterHandler(async _ => Close()));
            });
        }
    }
}