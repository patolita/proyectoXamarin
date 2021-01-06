namespace Arduino.Views    
{
    using Arduino;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterArduinoPage : MasterDetailPage
    {
        public MasterArduinoPage()
        {
            InitializeComponent();
            App.Navigator = Navigator;
            App.Master = this;
        }

    }
}