using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HipChat;
using System.Timers;
using Newtonsoft.Json;

namespace OctoBot
{
    class HipChatService
    {

        #region "Data Members"

        //Should default to empty. Set should come from the COnfigFile
        static private string APIKEY
        {
            get { return _apikey; }
            set { _apikey = value; }
        }
        private static string _apikey = string.Empty;

        public string HIPCHATUSERNAME { get { return "Octobot"; } }
        public int HIPCHATROOM { get { return 31845; } }
 
        public static bool CHAT_INITERFACE_ACTIVE {get; set;}
        public static bool OPERATION_ACTIVE { get; set; }

        static HipChatClient theClient;


        static Timer ticker = new Timer();

        // not implemented yet

        //static List<string> OperationsList;

        // Data Structures for BOT AI
        //private struct knownUsers
        //{
        //    string userName;
        //    int userId;
        //}

        //in list format
        //static List<knownUsers> userList;





        #endregion

        #region "Constructor"

        public HipChatService()
        {
            // setup the bot with defaults
            OPERATION_ACTIVE = false;


            theClient = new HipChatClient(APIKEY, HIPCHATROOM, HIPCHATUSERNAME);
            ticker.Elapsed += new ElapsedEventHandler(getChatHistory);
            ticker.Interval = 60000;

            try
            {
                //theClient.SendMessage("Octobot online...");
                Console.WriteLine(DateTime.Now + "[Chat] Initialized");
                CHAT_INITERFACE_ACTIVE = true;
            }
            catch 
            {
                Console.WriteLine(DateTime.Now + "[CHAT] Unable to initialize");
                CHAT_INITERFACE_ACTIVE = false;
            }

        }

        public HipChatService(string setAPIKEY)
        {
            APIKEY = setAPIKEY;
            OPERATION_ACTIVE = false;
            ticker.Elapsed += new ElapsedEventHandler(getChatHistory);
            ticker.Interval = 60000;

            theClient = new HipChatClient(APIKEY, HIPCHATROOM, HIPCHATUSERNAME);

            getRoomList();

            try
            {
                Console.WriteLine(DateTime.Now + "[Chat] Initialized");
                CHAT_INITERFACE_ACTIVE = true;
            }
            catch
            {
                Console.WriteLine(DateTime.Now + "[CHAT] Unable to initialize");
                CHAT_INITERFACE_ACTIVE = false;
            }


        }

        #endregion

        #region "Events"

        // the timer
        public void Tick()
        {
            if (!ticker.Enabled)
            {
                ticker.Start();
            }
        }

        private void getChatHistory(object source, ElapsedEventArgs e)
        {
            if (OPERATION_ACTIVE == false)
            {
                OPERATION_ACTIVE = true;
                theClient.SendMessage("tick!");
                parseRoomHistory(theClient.RoomHistory(DateTime.Now));
                OPERATION_ACTIVE = false;
            }
            else
            {
                return;
            }
        }

        private void getRoomList()
        {
            
        }

        #endregion
        
        
        #region "Bot Innard Logic"



        public void parseRoomHistory(string responseBody)
        {
            if (responseBody.Contains("!Octobot"))
            {
                theClient.SendMessage("What?");
            }
            else if (responseBody.Contains("!Octobot @google"))
            {
                Services.Gbot gbot = new Services.Gbot();
                //gbot.Search(this.); implement after the room history is parsed as a data structure
            }

        }





        #endregion



    }// end of class hipchat
}
