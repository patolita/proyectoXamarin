
namespace Arduino.ViewModels
{
    using Arduino.Helpers;
    using Arduino.Views;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public ObservableCollection<MenuItemViewModel> Menus 
        { 
            get; 
            set; 
        }
        public List<ArduinoModels> ArduinoList 
        { 
            get; 
            set; 
        }
        public  string UserName { 
            get;  
            set; 
        }
        public string Token 
        { 
            get;
            set; 
        }
        public string TokenType
        {
            get;
            set;
        }
        public LoginViewModel Login
        {
            get;
            set;
        }
        public RegisterViewModel Register 
        { 
            get; 
            set; 
        }
        public ArduinoPageViewModel ArduinoModels
        {
            get;
            set;
        }
        public ArduinoModelViewModel ArduinoModel 
        { 
            get; 
            set; 
        }
        public MyProfileViewModel MyProfile 
        { 
            get; 
            set; 
        }
        public AboutPage About
        {
            get;
            set;
        }
        public ChangePasswordViewModel ChangePassword 
        { 
            get; 
            set; 
        }
        public AboutViewModel About 
        { 
            get; 
            set; 
        }
        public string UserLocal
        {
            get;
            set;
        }
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            this.LoadMenu();
        }
        

        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {

                return   new MainViewModel();

            }
            return instance;        
         }

        private void LoadMenu()
        {
            this.Menus = new ObservableCollection<MenuItemViewModel>();
            this.Menus.Add(new MenuItemViewModel
            {
                Icon= "ic_settings",
                PageName= "MyProfilePage",
                Title="My Profile",
                

            });
            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_insert_chart_outlined",
                PageName = "StaticsPage",
                Title = "Statics"

            });
            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "MainPage",
                Title = "LogOut"

            });
            
            
        }
        public ICommand ShareCommand
        {
            get
            {
                return new RelayCommand(MyShare);
            }
        }

        private async void MyShare()
        {
            var opc = await Application.Current.MainPage.DisplayActionSheet(
                    "",
                    "",
                    "",
                    "Compartir",
                    "Calificar",
                    "Acerca de");

            switch (opc)
            {
                case "Compartir":
                    {
                        await Share.RequestAsync(new ShareTextRequest
                        { 
                            Uri= "https://play.google.com/store",
                            Title="MiApp"
                        });
                        break;
                    }
                case "Calificar":
                    {
                        await Browser.OpenAsync("https://play.google.com/store", BrowserLaunchMode.SystemPreferred);
                        break;
                    }
                case "Acerca de":
                    {
                        MainViewModel.GetInstance().About = new AboutViewModel();
                        App.Navigator.PushAsync(new AboutPage());
                        break;
                    }
            }

            /*await Application.Current.MainPage.DisplayAlert(
                "",
                opc,
                "Ok");*/
        }
    }
}
