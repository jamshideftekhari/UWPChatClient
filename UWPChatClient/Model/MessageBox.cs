using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPChatClient.Model
{
    public class MessageBox
    {
        private static MessageBox _instance;

        public static MessageBox Instance
        {
            get { return _instance ?? (_instance = new MessageBox()); }
        }

        public ObservableCollection<Client> ClientMessages { get; set; }

    }
}
