using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using UWPChatClient.Annotations;
using UWPChatClient.Common;
using UWPChatClient.Connection;
using UWPChatClient.Model;

namespace UWPChatClient.ViewModel
{
    class ClientVM : INotifyPropertyChanged
    {
        public Client ChatClient { get; set; }
       public ObservableCollection<string> ChatClientBuffer { get; set; }
        public string ChatMessage { get; set; }
        private ICommand _sendMessageCommand;
        private ICommand _startClientCommand;
        public TCPConnection ClientConnection { get; set; }

        public ICommand SendMessageCommand
        {
            get { return _sendMessageCommand ?? (_sendMessageCommand = new RelayCommand(SendMessage)); }
            set { _sendMessageCommand = value; }
        }

        public ICommand StartClientCommand
        {
            get { return _startClientCommand ?? (_startClientCommand = new RelayCommand(StartClient)); }
        }

        public void SendMessage()
        {
            ChatClient.Message(ChatMessage);
            ClientConnection.StartClient(ChatClient);
        }

        public void StartClient()
        {
            ClientConnection.StartClient(ChatClient);
        }

        public ClientVM()
        {
            ChatClientBuffer = new ObservableCollection<string>();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            ChatClient = new Client();
            ClientConnection = new TCPConnection(this);
            
        }

        public async void AddToBuffer(string clientReceivedResp)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority.Normal), () => {ChatClientBuffer.Add(clientReceivedResp);});
        } 


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
