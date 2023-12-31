using MVMCrudDemo.Models;
using MVMCrudDemo.ViewModel;

namespace MVMCrudDemo.Pages;

public partial class EditPage : ContentPage
{
    public EditPage()
    {
    }

    public EditPage(UserInfo userInfo)
	{
		InitializeComponent();
        BindingContext = new EditPageViewModel(Navigation, userInfo);
    }
}