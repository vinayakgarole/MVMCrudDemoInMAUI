using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MVMCrudDemo.Models;
using Newtonsoft.Json;

namespace MVMCrudDemo.ViewModel
{
    public class EditPageViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _price;
        private string _selectedName;
        private readonly INavigation _navigation;

        public ICommand EditCommand { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedName
        {
            get => _selectedName;
            set
            {
                if (_selectedName != value)
                {
                    _selectedName = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> Names { get; set; }
        public UserInfo UserInfo { get; set; }

        public EditPageViewModel(INavigation navigation, UserInfo userInfo)
        {
            _navigation = navigation;
            UserInfo = userInfo;
            Name = userInfo.Name;
            Description = userInfo.Description;
            Price = userInfo.Price.ToString();

            Names = new List<string>
            {
                "Jacob",
                "Michael",
                "Matthew",
                "Jennifer",
                "Michelle"
            };

            //EditCommand = new Command(async () => await EditAsync());
            SelectedName = userInfo.Name;
        }

        public async Task EditAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(SelectedName))
                {
                    if (decimal.TryParse(Price, out decimal price))
                    {
                        UserInfo.Name = SelectedName;
                        UserInfo.Description = Description;
                        UserInfo.Price = price;

                        var jsonContent = JsonConvert.SerializeObject(UserInfo);
                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/api/Products/api/update");
                        request.Headers.Add("accept", "*/*");
                        var content = new StringContent(jsonContent, null, "application/json");
                        request.Content = content;
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        var result = await response.Content.ReadAsStringAsync();
                        ResponseModel<UserInfo> model = JsonConvert.DeserializeObject<ResponseModel<UserInfo>>(result);

                        await App.Current.MainPage.DisplayAlert("Success", "Record has been updated successfully", "OK");
                        await _navigation.PopAsync();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Invalid Price", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Please select a Name", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const string BaseUrl = "https://localhost:7077"; // Define your BaseUrl here
    }
}
