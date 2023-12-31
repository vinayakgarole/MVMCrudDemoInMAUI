using MVMCrudDemo.ViewModel;

namespace MVMCrudDemo.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		 InitializeComponent();
        BindingContext = new HomePageViewModel(Navigation);
	}
}