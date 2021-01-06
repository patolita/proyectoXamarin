
namespace Arduino.ViewModels
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Services;
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;

    public class ArduinoPageViewModel : BaseViewModel
    {
        private ApiService apiService;
        
        private ObservableCollection<ArduinoItemViewModel> arduino;
        private bool isRefreshing;
        private string filter;
        
        public ObservableCollection<ArduinoItemViewModel> Arduino
        {
            get { return this.arduino; }
            set { this.SetValue(ref this.arduino, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public string Filter
        {
            get { return this.filter; }
            set { 
                this.SetValue(ref this.filter, value);
                this.Search();
            }
        }
        

        public ArduinoPageViewModel()
        {
            this.apiService = new ApiService();
            this.LoadArduinos();
        }

       
        private async void LoadArduinos()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();

                return;
            }

            var response = await this.apiService.GetList<ArduinoModels>("https://restcountries.eu", "/rest","/v2/all");

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            MainViewModel.GetInstance().ArduinoList = (List<ArduinoModels>)response.Result;
            this.Arduino = new ObservableCollection 
                <ArduinoItemViewModel>
                (this.ToArduinoItemViewModel());
            this.IsRefreshing = false;
        }

        private IEnumerable<ArduinoItemViewModel> ToArduinoItemViewModel()
        {
            return MainViewModel.GetInstance().ArduinoList.Select(l => new ArduinoItemViewModel
            {
                Alpha2Code = l.Alpha2Code,
                Alpha3Code = l.Alpha3Code,
                AltSpellings = l.AltSpellings,
                Area = l.Area,
                Borders = l.Borders,
                CallingCodes = l.CallingCodes,
                Capital = l.Capital,
                Cioc = l.Cioc,
                Currencies = l.Currencies,
                Demonym = l.Demonym,
                Flag = l.Flag,
                Gini = l.Gini,
                Languages = l.Languages,
                Latlng = l.Latlng,
                Name = l.Name,
                NativeName = l.NativeName,
                NumericCode = l.NumericCode,
                Population = l.Population,
                Region = l.Region,
                RegionalBlocs = l.RegionalBlocs,
                Subregion = l.Subregion,
                Timezones = l.Timezones,
                TopLevelDomain = l.TopLevelDomain,
                Translations = l.Translations,
            });
        }

        public ICommand RefreshCommand 
        {
            get
            {
                return new RelayCommand(LoadArduinos);
            } 
            
        }
        public ICommand SearchCommand 
        {
            get
            {
                return new RelayCommand(Search);
            }
          
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
               this.Arduino = new ObservableCollection 
                <ArduinoItemViewModel>
                (this.ToArduinoItemViewModel());
            }
            else
            {
                this.Arduino = new ObservableCollection
                <ArduinoItemViewModel>
                (this.ToArduinoItemViewModel().Where(a => a.Name.ToLower().Contains(this.Filter.ToLower())||
                    a.Capital.ToLower().Contains(this.Filter.ToLower())));

            }
        }

        public static implicit operator ArduinoPageViewModel(ArduinoModelViewModel v)
        {
            throw new NotImplementedException();
        }
    }

}
