using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace ReactiveUIApp.ViewModels;

public class MainViewModel : ReactiveObject, IActivatableViewModel
{
    private const string CancelAreYouSure = "Cancel, are you sure?";
    private const string DoYouWantToSave = "Do you want to save";
    private const string Saved = "Saved";
    private const string Cancelled = "Cancelled";
    private string _city;
    private string _dob;

    private string _firstName;
    private string _lastName;
    private string _message;
    private string _state;
    private string _street;
    private string _zip;

    public MainViewModel()
    {
        var canExecute = this.WhenAnyValue(
            x => x.FirstName,
            x => x.LastName,
            x => x.Dob,
            x => x.Street,
            x => x.City,
            x => x.State,
            x => x.Zip,
            (firstName, lastName, dob, street, city, state, zip) =>
                !string.IsNullOrWhiteSpace(firstName) ||
                !string.IsNullOrWhiteSpace(lastName) ||
                !string.IsNullOrWhiteSpace(dob) ||
                !string.IsNullOrWhiteSpace(street) ||
                !string.IsNullOrWhiteSpace(city) ||
                !string.IsNullOrWhiteSpace(state) ||
                !string.IsNullOrWhiteSpace(zip));

        SaveCommand = ReactiveCommand.CreateFromTask(DoSave, canExecute);
        CancelCommand = ReactiveCommand.CreateFromTask(DoCanccel);
        ExitCommand = ReactiveCommand.CreateFromTask(DoExit);
        ShowMessage = new Interaction<string, bool>();
        ShowConfirmation = new Interaction<string, bool>();
        CloseApp = new Interaction<Unit, Unit>();
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; set; }

    public Interaction<string, bool> ShowMessage { get; }
    public Interaction<string, bool> ShowConfirmation { get; }
    public Interaction<Unit, Unit> CloseApp { get; }

    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    public string FirstName
    {
        get => _firstName;
        set => this.RaiseAndSetIfChanged(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => this.RaiseAndSetIfChanged(ref _lastName, value);
    }

    public string Dob
    {
        get => _dob;
        set => this.RaiseAndSetIfChanged(ref _dob, value);
    }

    public string Street
    {
        get => _street;
        set => this.RaiseAndSetIfChanged(ref _street, value);
    }

    public string City
    {
        get => _city;
        set => this.RaiseAndSetIfChanged(ref _city, value);
    }

    public string State
    {
        get => _state;
        set => this.RaiseAndSetIfChanged(ref _state, value);
    }

    public string Zip
    {
        get => _zip;
        set => this.RaiseAndSetIfChanged(ref _zip, value);
    }

    public ViewModelActivator Activator { get; } = new();

    private void ClearFields()
    {
        FirstName = null;
        LastName = null;
        Dob = null;
        Street = null;
        City = null;
        State = null;
        Zip = null;
    }

    private async Task DoCanccel()
    {
        if (await ShowConfirmation.Handle(CancelAreYouSure))
        {
            ClearFields();
            Message = Cancelled;
        }
    }

    private async Task DoExit()
    {
        if ((SaveCommand as ICommand).CanExecute(null))
            if (await ShowConfirmation.Handle(DoYouWantToSave))
                await DoSave();
        await CloseApp.Handle(Unit.Default);
    }

    private async Task DoSave()
    {
        ClearFields();
        await ShowMessage.Handle(Saved);
        Message = Saved;
    }
}