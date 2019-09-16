using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPChatClient.Model
{
    public class Client
    {
        public string PortNr { get; set; }
        public string SentMessage { get; set; }
        public string ReceivedMessage { get; set; }
        public string ClientStatus { get; set; }

        public Client()
        {
            PortNr = "6789";
            //PortNr = "1338";
            SentMessage = "Test Message";
            ReceivedMessage = "";
            ClientStatus = "Server is listening";
        }

        public void Message(string sentMessage)
        {
            SentMessage = sentMessage;
        }
    }
}
