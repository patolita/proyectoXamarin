namespace Arduino.ViewModels
{
    using Arduino.Helpers;
    using Arduino.Views;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class MenuItemViewModel
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        public string UserName { get; set; }

        public ICommand NavigateCommand 
        {
            get
            {
                return new RelayCommand(Navigate);
            } 
             
        }

        private void Navigate()
        {
            App.Master.IsPresented = false;
            if(this.PageName == "MainPage")
            {
                Settings.Token = string.Empty;
                Settings.TokenType = string.Empty;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = string.Empty;
                mainViewModel.TokenType = string.Empty;
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else if(this.PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new  MyProfileViewModel();
                App.Navigator.PushAsync(new MyProfilePage());
            }
        }
    }
}
