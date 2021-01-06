namespace Arduino.ViewModels
{
    using Arduino.Helpers;
    using Arduino.Models;
    using Arduino.Services;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class ChangePasswordViewModel : BaseViewModel
    {

        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public User User
        {
            get;
            set;
        }
        public string CurrentPassword
        {
            get;
            set;
        }

        public string NewPassword
        {
            get;
            set;
        }

        public string Confirm
        {
            get;
            set;
        }

        public ChangePasswordViewModel()
        {
            var userloc = Settings.UserLocal;
            User = JsonConvert.DeserializeObject<User>(userloc);
            apiService = new ApiService();
            IsEnabled = true;
            CurrentPassword = User.Password;


        }

        public ICommand ChangePasswordCommand
        {
            get
            {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword()

        {
            if (string.IsNullOrEmpty(CurrentPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Password Validation",
                    "Accept");
                return;
            }

            if (CurrentPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Your password is not 6 characters long",
                    "Accept");
                return;
            }

            if (CurrentPassword != User.Password)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Current password does not match",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "PasswordValidation",
                    "Accept");
                return;
            }

            if (NewPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "our password is not 6 characters long",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Confirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "ConfirmValidation",
                    "Accept");
                return;
            }

            if (NewPassword != Confirm)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Password does not match",
                    "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var request = new ChangePasswordRequest
            {
                CurrentPassword = CurrentPassword,
                Email = User.Email,
                NewPassword = NewPassword,
            };

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.ChangePassword(
                apiSecurity,
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token,
                request);


              if (!response.IsSuccess)
              {
                  IsRunning = false;
                  IsEnabled = true;
                  await Application.Current.MainPage.DisplayAlert(
                      "Error",
                      "Error Changing Password",
                      "Accept");
                  return;
              }




            var userApi = await this.apiService.GetUserByEmail(
                apiSecurity,
                this.User.Email
                );

            User.Password = NewPassword;
            var userloc = JsonConvert.SerializeObject(User);
            MainViewModel.GetInstance().UserLocal = userloc;


            IsRunning = false;
            IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                "ConfirmLabel",
                "ChagePasswordConfirm",
                "Accept");
            await App.Navigator.PopAsync();
        }
    }
}
