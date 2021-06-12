using System;
using System.Drawing;
using System.Windows.Forms;
using ReactiveUI;
using WinFormMarkup.Extensions;

namespace ReactiveUIApp.Views
{
    public partial class MainView
    {
        private TextBox _city;
        private Button _clearButton;
        private DateTimePicker _dob;
        private ToolStripMenuItem _exitMenu;
        private TextBox _firstName;
        private TextBox _lastName;
        private Button _saveButton;
        private TextBox _state;
        private ToolStripStatusLabel _statusLabel;
        private TextBox _street;
        private TextBox _zip;

        private void Bind()
        {
            this.WhenActivated(d =>
            {
                d(this.BindCommand(ViewModel, vm => vm.ExitCommand, v => v._exitMenu));
                d(this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v._saveButton));
                d(this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v._clearButton));
                d(this.Bind(ViewModel, vm => vm.FirstName, v => v._firstName.Text));
                d(this.Bind(ViewModel, vm => vm.LastName, v => v._lastName.Text));
                d(this.Bind(ViewModel, vm => vm.Dob, v => v._dob.Value,
                    s => s == null ? DateTime.Today : DateTime.Parse(s),
                    d => d == DateTime.Today ? null : d.ToShortDateString()));
                d(this.Bind(ViewModel, vm => vm.Street, v => v._street.Text));
                d(this.Bind(ViewModel, vm => vm.City, v => v._city.Text));
                d(this.Bind(ViewModel, vm => vm.State, v => v._state.Text));
                d(this.Bind(ViewModel, vm => vm.Zip, v => v._zip.Text));
                d(this.Bind(ViewModel, vm => vm.Message, v => v._statusLabel.Text));
                d(ViewModel.closeApp.RegisterHandler(_ => Application.Exit()));
                d(ViewModel.showMessage.RegisterHandler(msg =>
                {
                    var result = MessageBox.Show(msg.Input, "Message", MessageBoxButtons.OK);
                    msg.SetOutput(true);
                }));
                d(ViewModel.showConfirmation.RegisterHandler(msg =>
                {
                    var result = MessageBox.Show(msg.Input, "Confirm", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);
                    msg.SetOutput(result == DialogResult.OK);
                }));
            });
        }

        private void Build()
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
                            _statusLabel = new ToolStripStatusLabel()
                                .Text("Status Label")),
                    new TableLayoutPanel()
                        .TableLayout(2, 8)
                        .ColumnStyles("30%|70%")
                        .RowStyles("*|*|*|*|*|*|*|*")
                        .Padding(30)
                        .Dock(DockStyle.Fill)
                        .TableControls(
                            new TableLocation(0, 0, new Label().Text("First &Name:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 0, _firstName = new TextBox()
                                .Dock(DockStyle.Fill)),
                            new TableLocation(0, 1, new Label().Text("&Last Name:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 1, _lastName = new TextBox()
                                .Dock(DockStyle.Fill)),
                            new TableLocation(0, 2, new Label().Text("&Birthday:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 2, _dob = new DateTimePicker()
                                .CustomFormat("dd/MM/yyyy")),
                            new TableLocation(0, 3, new Label().Text("S&treet:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 3, _street = new TextBox()
                                .Dock(DockStyle.Fill)),
                            new TableLocation(0, 4, new Label().Text("Cit&y:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 4, _city = new TextBox()
                                .Dock(DockStyle.Fill)),
                            new TableLocation(0, 5, new Label().Text("State:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 5, _state = new TextBox()
                                .Dock(DockStyle.Fill)),
                            new TableLocation(0, 6, new Label().Text("&Zip:")
                                .Dock(DockStyle.Fill).TextAlign(ContentAlignment.MiddleRight)),
                            new TableLocation(1, 6, _zip = new TextBox()
                                .Dock(DockStyle.Fill)),
                            new TableLocation(0, 7, 2, 1,
                                new FlowLayoutPanel()
                                    .FlowDirection(FlowDirection.RightToLeft)
                                    .WrapContents(false)
                                    .Dock(DockStyle.Fill)
                                    .Controls(
                                        _saveButton = new Button().Text("&Save"),
                                        _clearButton = new Button().Text("&Clear")))
                        )
                )
                .AutoSize(true)
                .CancelButton(_clearButton)
                .AcceptButton(_saveButton)
                .StartPosition(FormStartPosition.CenterScreen)
                .Text("Basic Test App");
        }
    }
}