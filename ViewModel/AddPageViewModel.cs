using MVMCrudDemo.Models;
using MVMCrudDemo.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVMCrudDemo.ViewModel
{
    public class AddPageViewModel
    {
        public static string BaseUrl = "https://localhost:7077";
        public List<string> Names { get; set; }
        public string SelectedName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public INavigation Navigation { get; set; }

        public AddPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Names = new List<string>();
            Description = string.Empty;
            Price = string.Empty;
            Names = new List<string>
            {
                "Select User",
                "Jacob",
                "Michael",
                "Matthew",
                "Jennifer",
                "Michelle"
            };

            SelectedName = "Select User";
            Description = "";
            Price = "";

            SaveCommand = new Command(async () => await Insert());
            BackCommand = new Command(async () => await Back_Clicked());


        }

        private async Task Insert()
        {
            try
            {
                // Validate other fields as before
                if (string.IsNullOrWhiteSpace(Description) || !decimal.TryParse(Price, out decimal price))
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Please Fill in all details ", "OK");
                    return;
                }

                // Get the selected name from the Picker
                string selectedName = SelectedName;

                if (string.IsNullOrWhiteSpace(selectedName) || selectedName == "Select User")
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Please select a Name", "OK");
                    return;
                }

                UserInfo u = new UserInfo
                {
                    Name = selectedName,
                    Description = Description,
                    Price = price
                };

                // The rest of your code remains the same
                var jsonContent = JsonConvert.SerializeObject(u);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/api/Products/api/create");
                request.Headers.Add("accept", "*/*");
                var content = new StringContent(jsonContent, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                ResponseModel<UserInfo> model = JsonConvert.DeserializeObject<ResponseModel<UserInfo>>(result);

                await App.Current.MainPage.DisplayAlert("Success", "Record has been saved successfully", "Ok");
                await App.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "Ok");
            }
        }

        private async Task Back_Clicked()
        {
          
            App.Current.MainPage = new HomePage();
        }

        //private void Save_Clicked(object sender, EventArgs e)
        //{
        //    Insert();
        //}
    }
}
