namespace Arduino
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Helpers;
    using Arduino.ViewModels;
    using Arduino.Views;
    using Arduino.Models;

    public partial class App : Application
    {
        public static NavigationPage Navigator { 
            get; 
            internal set; 
        }
        public static MasterArduinoPage Master 
        { 
            get; 
            internal set; 
        }

        public App()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = Settings.Token;
                mainViewModel.TokenType = Settings.TokenType;
                mainViewModel.UserLocal = Settings.UserLocal;
                mainViewModel.ArduinoModels = new ArduinoPageViewModel();
                this.MainPage = new MasterArduinoPage();
            }
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
