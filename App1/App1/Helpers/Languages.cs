namespace Arduino.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Arduino.Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string EmailValidator
        {
            get { return Resource.EmailValidator; }
        }
        public static string Accept
        {
            get { return Resource.Accept; }
        }
        public static string PasswordValidator
        {
            get { return Resource.PasswordValidator; }
        }
    }
}

