

namespace Arduino.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Models;
    public class ArduinoModelViewModel : BaseViewModel
    {
        private ObservableCollection<Border> borders;
        private ObservableCollection<Currency> currencies;
        private ObservableCollection<Language> languages;
        public ArduinoModels ArduinoModel
        { 
            get; 
            set; 
        }
        public ObservableCollection<Border> Borders
        {
            get { return this.borders; }
            set { this.SetValue(ref this.borders, value); }
        }
        public ObservableCollection<Currency> Currencies
        {
            get { return this.currencies; }
            set { this.SetValue(ref this.currencies, value); }
        }
        public ObservableCollection<Language> Languages
        {
            get { return this.languages; }
            set { this.SetValue(ref this.languages, value); }
        }
        public ArduinoModelViewModel(ArduinoModels arduinoModel)
        {
            this.ArduinoModel = arduinoModel;
            this.LoadBorders();
            this.Currencies = new ObservableCollection<Currency>(this.ArduinoModel.Currencies);
            this.Languages = new ObservableCollection<Language>(this.ArduinoModel.Languages);
        }

        private void LoadBorders()
        {
            this.Borders = new ObservableCollection<Border>();
            foreach(var border in this.ArduinoModel.Borders)
            {
                var arduino = MainViewModel.GetInstance().ArduinoList.Where(l => l.Alpha3Code == border).FirstOrDefault();
                if(arduino != null)
                {
                    this.Borders.Add(new Border
                    {
                        Code = arduino.Alpha3Code,
                        Name = arduino.Name,

                    });
                }
            }
        }
    }
}
