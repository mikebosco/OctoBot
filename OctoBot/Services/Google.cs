using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.API.Search;

namespace OctoBot.Services
{
    class Gbot
    {
        public string Search(string query)
        {
            GwebSearchClient gweb = new GwebSearchClient("http://ubound.hipchat.com");
            
            var results = gweb.Search(query, 1);
            
            try
            {
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        return result.Url;
                    }
                }
            }
            catch
            {
                return "Error in search!";
            }

            return "Whoa! Error in program logic... exiting gracefully";

        } // end of constructor

    }
}
