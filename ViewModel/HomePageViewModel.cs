using MVMCrudDemo.Models;
using MVMCrudDemo.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace MVMCrudDemo.ViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        private ObservableCollection<UserInfo> _userInfos;
        public ObservableCollection<UserInfo> UserInfos
        {
            get { return _userInfos; }
            set
            {
                _userInfos = value;
                OnPropertyChanged(nameof(UserInfos));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get;  set; }
        public ICommand DeleteCommand { get; }

        public INavigation Navigation { get; set; }

        public HomePageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            UserInfos = new ObservableCollection<UserInfo>(); // Initialize UserInfos here
            AddCommand = new Command(async () => await AddCommandDataGet());
            EditCommand = new Command<int>(async (Id) => await ExecuteEditCommand(Id));
            DeleteCommand = new Command<int>(async (parameter) => await ExecuteDeleteCommand(parameter));
            login();
        }


        private async Task AddCommandDataGet()
        {
           

            App.Current.MainPage = new AddPage();
        }

        private async Task ExecuteEditCommand(int Id)
        {
            App.Current.MainPage = new EditPage();
        }

        private async Task ExecuteDeleteCommand(int Id)
        {
            var answer = await App.Current.MainPage.DisplayAlert("Delete Confirmation", "Are you sure you want to delete this record?", "Yes", "No");

            if (answer)
            {
                var userId = Id;
                await DeleteAsync(userId);
            }
        }

        private async Task DeleteAsync(int Id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"https://localhost:7077/api/Products/api/products/delete/{Id}";
                    var response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // The DELETE request was successful, so refresh the user list.
                        login();
                    }
                    else
                    {
                        // Handle non-success status codes here
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request
            }
        }

        public async void login()
        {
            var client = new HttpClient();
            string url = "https://localhost:7077/api/Products/api/products";
            client.BaseAddress = new Uri(url);

            try
            {
                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<UserInfo> userInfos = JsonConvert.DeserializeObject<List<UserInfo>>(content);

                    UserInfos.Clear();
                    foreach (var user in userInfos)
                    {
                        UserInfos.Add(user);
                    }
                }
                else
                {
                    // Handle non-success status codes here
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request
            }
        }
    }
}
