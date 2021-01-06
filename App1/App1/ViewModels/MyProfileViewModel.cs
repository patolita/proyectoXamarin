
namespace Arduino.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using Views;
    using Xamarin.Forms;

    public class MyProfileViewModel : BaseViewModel
    {
       
        private ApiService apiService;
        
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;

        public User User
        {
            get;
            set;
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public MyProfileViewModel()
        {
            var userloc = Settings.UserLocal;
            User = JsonConvert.DeserializeObject<User>(userloc);
            apiService = new ApiService();
            ImageSource = this.User.ImageFullPath;
            IsEnabled = true;
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
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
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
            await App.Navigator.PushAsync(new ChangePasswordPage());
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter First Name",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter Last Name",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter Email",
                    "Accept");
                return;
            }

            if (!RegexUtilities.IsValidEmail(User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter valid email",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(User.Telephone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter Phone Number",
                    "Accept");
                return;
            }

            

            IsRunning = true;
            IsEnabled = false;

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    checkConnetion.Message,
                    "Accept");
                return;
            }

            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
            }

            var user = new User
            {
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Telephone = User.Telephone,
                ImageArray = imageArray,
                UserTypeId = 1,
                Password = User.Password,
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            //var cadena = "&email=" + this.User.Email + "&firstName=" + this.User.FirstName + "&lastName=" + this.User.LastName + "&telephone=" + this.User.Telephone;
            var response = await apiService.PutUser(
              apiSecurity,
              user,
              MainViewModel.GetInstance().TokenType,
              MainViewModel.GetInstance().Token);

            var userApi = await apiService.GetUserByEmail(
                apiSecurity,
                User.Email
                );

            var userloc = JsonConvert.SerializeObject(userApi);
            MainViewModel.GetInstance().UserLocal = userloc;
            Settings.UserLocal = userloc;
            User = JsonConvert.DeserializeObject<User>(userloc);
            ImageSource = User.ImageFullPath;

            /*if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }*/


            IsRunning = false;
            IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                "ConfirmLabel",
                "UserRegisteredMessage",
                "Accept");
            await App.Navigator.PopAsync();
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    "Source Image",
                    "Cancel",
                    null,
                    "Gallery",
                    "Camera");

                if (source == "Cancel")
                {
                    file = null;
                    return;
                }

                if (source == "FromCamera")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }



    }
}

