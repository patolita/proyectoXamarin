namespace Arduino.Helpers
{
    using Arduino.Models;
    using Newtonsoft.Json;
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    using Xamarin.Essentials;

    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const string token = "Token";
        const string tokenType = "TokenType";
        const string userLocal = "UserLocal";
        static readonly string stringDefault = string.Empty;
        


        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(token, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(token, value);
            }
        }
        
        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }
        
        public static string UserLocal
        {
            get
            {
                return AppSettings.GetValueOrDefault(userLocal, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userLocal, value);
            }
        }
        /* public static User UserLocal
         {
             get
             {
                 var userKey=(string)JsonConvert.DeserializeObject<User>(userLocal);
                 return AppSettings.GetValueOrDefault(userKey, stringDefault);
             }
             set
             {
                 AppSettings.AddOrUpdateValue(userLocal, value);
             }
         }*/



    }
}

