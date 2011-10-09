using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OctoBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // set the services

            HipChatService hipchat = new HipChatService(ConfigFile.getHipchatKey());
            while (HipChatService.CHAT_INITERFACE_ACTIVE == true)
            {
                hipchat.Tick();
            }


        }
    }


}
