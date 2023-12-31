using MVMCrudDemo.ViewModel;

namespace MVMCrudDemo.Pages;

public partial class AddPage : ContentPage
{
	public AddPage()
	{
		InitializeComponent();
        BindingContext = new AddPageViewModel(Navigation);
		}     
}