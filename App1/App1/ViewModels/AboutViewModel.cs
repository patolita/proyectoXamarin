using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace Arduino.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private bool _searchBarFocused;

        public bool SearchBarFocused
        {
            get { return _searchBarFocused; }
            set
            {
                _searchBarFocused = value;
                base.OnPropertyChanged("SearchBarFocused");
            }
        }

        public async Task InitializeData()
        {
            // Other operations...

            SearchBarFocused = true;
        }
    }
}
