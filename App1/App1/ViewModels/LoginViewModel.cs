


namespace Arduino.ViewModels
{
    using System;
    using System.Windows.Input;
    using System.ComponentModel;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Arduino.Views;
    using Arduino.Services;
    using Arduino.Helpers;
    using Arduino.Models;
    using Newtonsoft.Json;

    public class LoginViewModel : BaseViewModel
    {
        private ApiService apiService;
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
  
        public string Email 
        {
            get { return this.email; }
            set { this.SetValue(ref this.email, value); }
        }
        
        public string Password 
        {
            get { return this.password; }
            set { this.SetValue(ref this.password, value); }
        }
        public bool IsRemenber 
        { 
            get; 
            set; 
        }
        public bool IsRunning {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public ICommand LoginCommand 
        { 
            get
            {
                return new RelayCommand(Login);
            }
           
        }
        public ICommand RegisterCommand 
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        

        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsRemenber = true;
            this.IsEnabled = true;
            this.Email = "paolazorrila16@gmail.com";
            this.Password = "123456";
        }

        private async void Login()
        {
           if(string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.EmailValidator,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidator,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var token = await this.apiService.GetToken(
                apiSecurity,
                this.Email,
                this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    "Something was wrong, please try later.",
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    token.ErrorDescription,
                    Languages.Accept);
                this.Password = string.Empty;
                return;
            }
            /* if (this.Email != "paolazorrila16@gmail.com" || this.Password !="1234")
             {
                 this.IsRunning = false;
                 this.IsEnabled = true;
                 await Application.Current.MainPage.DisplayAlert(
                     "Error",
                     "Email or Password incorrect",
                     "Accept");e envia ee
                 this.Password = string.Empty;
                 return;
             }*/

            var userResp = await this.apiService.GetUserByEmail(
               apiSecurity,
               this.Email);
            var userloc =  JsonConvert.SerializeObject(userResp);
            

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;
            mainViewModel.UserLocal = userloc;

            if (this.IsRemenber)
            {
                Settings.Token = token.AccessToken;
                Settings.TokenType = token.TokenType;
                Settings.UserLocal = userloc;

            }
            
            mainViewModel.ArduinoModels = new ArduinoPageViewModel();
            //await Application.Current.MainPage.Navigation.PushAsync(new ArduinoPage());
            Application.Current.MainPage = new MasterArduinoPage();
            
            this.IsRunning = false;
            this.IsEnabled = true;

            this.Password = string.Empty;
            this.Email = string.Empty;

            

        }
        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }


    }
}
