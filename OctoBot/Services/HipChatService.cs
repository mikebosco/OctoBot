using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HipChat;
using HipChat.Entities;
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
        private static bool FIRSTRUN = true;

        static HipChatClient theClient;


        static Timer ticker = new Timer();

        // not implemented yet

       static List<Operands> OperationsList = new List<Operands>();

        // Data Structures for BOT AI
        private struct Operands
        {
            public string userId;
            public string command;
            public DateTime date;
        }

        //in list format
        //static List<knownUsers> userList;





        #endregion

        #region "Constructor"

        public HipChatService(string setAPIKEY)
        {
            APIKEY = setAPIKEY;
            OPERATION_ACTIVE = false;
            ticker.Elapsed += new ElapsedEventHandler(getChatHistory);
            ticker.Interval = 60000;

            theClient = new HipChatClient(APIKEY, HIPCHATROOM, HIPCHATUSERNAME);

            try
            {
                Console.WriteLine(DateTime.Now + "[Chat] Initialized");
                parseRoomHistory(theClient.ListHistoryAsNativeObjects());
                // set the firstrun flag to false 
                // doing so will prevent the buffer-overflow.
                FIRSTRUN = false;
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
                parseRoomHistory(theClient.ListHistoryAsNativeObjects());
                OPERATION_ACTIVE = false;
            }
            else
            {
                return;
            }
        }

        #endregion
        
        
        #region "Bot Innard Logic"



        public void parseRoomHistory(List<HipChat.Entities.Message> Messages)
        {
            foreach (var msg in Messages)
            {
                msg.Text = msg.Text.ToLower();
                if (msg.Text == "@octobot" || msg.Text == "@Octobot")
                {
                    if (checkIfCommandIssued(msg) == false && FIRSTRUN == false)
                    {
                        theClient.SendMessage("(troll) Can I help you " + msg.From.Name + "?");
                    }
                    // now that the command was sent, add it to the issued commands list
                    addToIssuedCommands(msg);
                    
                }
                else if (msg.Text.Contains("@octobot search"))
                {
                    if (checkIfCommandIssued(msg) == false && FIRSTRUN == false)
                    {
                        Services.Gbot gbot = new Services.Gbot();
                        string searchQuery = msg.Text.Substring(16);
                        theClient.SendMessage(gbot.Search(searchQuery));
                    }
                    // add it to the list
                    addToIssuedCommands(msg);
                }
                else if (msg.Text.Contains("@octobot image"))
                {
                    if (checkIfCommandIssued(msg) == false && FIRSTRUN == false)
                    {
                        string searchQuery = msg.Text.Substring(14);
                        theClient.SendMessage(Services.Gbot.ImageSearch(searchQuery));
                    }
                    // add it to the list
                    addToIssuedCommands(msg);
                }

                else if (msg.Text == "@octobot refresh")
                {
                    if (checkIfCommandIssued(msg) == false && FIRSTRUN == false)
                    {
                        FIRSTRUN = true;
                        OperationsList.Clear();
                        parseRoomHistory(theClient.ListHistoryAsNativeObjects());
                        FIRSTRUN = false;
                        theClient.SendMessage("Ok boss, I've retallied my actions. Sorry for being annoying.");
                    }
                    // add it to the list
                    addToIssuedCommands(msg);
                }
                else if (msg.Text.Contains("@octobot define"))
                {
                    if (checkIfCommandIssued(msg) == false && FIRSTRUN == false)
                    {
                        Services.Gbot gbot = new Services.Gbot();
                        string searchQuery = msg.Text.Substring(15);
                        theClient.SendMessage(gbot.Search("define: " + searchQuery));
                    }
                    // add it to the list
                    addToIssuedCommands(msg);
                }


            }
               
           }
        


        private bool checkIfCommandIssued(HipChat.Entities.Message message)
        {

            //cast a temp struct with the message
            var op = new Operands();
            op.userId = message.From.Id;
            op.command = message.Text;
            op.date = message.Date;

            foreach (var completeOperation in OperationsList)
            {
                if (op.userId == completeOperation.userId && op.date == completeOperation.date 
                    && op.command == completeOperation.command)
                {
                    return true;
                }
            }

            return false;
        }

        private void addToIssuedCommands(HipChat.Entities.Message message)
        {
            var op = new Operands();
            op.userId = message.From.Id;
            op.command = message.Text;
            op.date = message.Date;
            OperationsList.Add(op);

        }


        #endregion



    }// end of class hipchat
}
