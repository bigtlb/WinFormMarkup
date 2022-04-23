using ReactiveUI;
using ReactiveUIApp.ViewModels;

namespace ReactiveUIApp.Views;

public partial class MainView : Form, IViewFor<MainViewModel>
{
    public MainView()
    {
        Build();
        Bind();
        ViewModel = new MainViewModel();
    }

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MainViewModel)value;
    }

    public MainViewModel ViewModel { get; set; }
}