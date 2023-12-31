using MVMCrudDemo.Pages;

namespace MVMCrudDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
        }
    }
}