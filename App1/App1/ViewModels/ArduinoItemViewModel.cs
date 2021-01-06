namespace Arduino.ViewModels
{
    using Arduino.Models;
    
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;
    public class ArduinoItemViewModel : ArduinoModels
    {
        public ICommand SelectArduinoModelsCommand 
        {
            get
            {
                return new RelayCommand(SelectArduinoModels);
            } 
             
        }

        private async void SelectArduinoModels()
        {
            MainViewModel.GetInstance().ArduinoModel = new ArduinoModelViewModel(this);
            await App.Navigator.PushAsync(new ArduinoTabbedPage());
            //await Application.Current.MainPage.Navigation.PushAsync
              //  (new ArduinoTabbedPage());
        }
    }
}
