using System;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Text;



namespace OctoBot
{
    public class ConfigFile 

    {
        
        // Static method to open the config file and read values
        public static string getHipchatKey()
        {
            return ConfigurationSettings.AppSettings["HipChatAPIKey"];

        } // end of LoadConfig

    }
}
