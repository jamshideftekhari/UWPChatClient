using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using UWPChatClient.Model;
using UWPChatClient.ViewModel;

namespace UWPChatClient.Connection
{
    class TCPConnection
    {
        private ClientVM ClientVmRef;
        public TCPConnection(ClientVM clientVm)
        {
            ClientVmRef = clientVm;
        }
        public async void StartClient(Client client)
        {
            // create StreamSocket and establish connection to the server
            try
            {
                using (var streamSocket = new StreamSocket())
                {
                    var hostName = new HostName("localhost");
                    client.ClientStatus = "Connecting to the server ....." + client.PortNr;
                    ClientVmRef.AddToBuffer(client.ClientStatus);

                    await streamSocket.ConnectAsync(hostName, client.PortNr);

                    ClientVmRef.AddToBuffer("Client Connected");

                    using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
                        {
                            using (var streamWriter = new StreamWriter(outputStream))
                            {
                                await streamWriter.WriteLineAsync(client.SentMessage);
                                await streamWriter.FlushAsync();
                            }
                            ClientVmRef.AddToBuffer(String.Format("send message: \"{0}\"", client.SentMessage));

                            using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                            {
                                using (var streamReader = new StreamReader(inputStream))
                                {
                                    client.ReceivedMessage = await streamReader.ReadLineAsync();
                                }
                            }
                            ClientVmRef.AddToBuffer(string.Format("Received message: \"{0}\"", client.ReceivedMessage));
                        }
                    

                    ClientVmRef.AddToBuffer("Client closed its socket");

                }
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                client.ClientStatus = webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message;
                ClientVmRef.AddToBuffer(client.ClientStatus);
            }

        }

        //public async void StartClientTest(Client client)
        //{
        //    // create StreamSocket and establish connection to the server
        //    try
        //    {
        //        using (var streamSocket = new StreamSocket())
        //        {
        //            var hostName = new HostName("localhost");
        //            client.ClientStatus = "Connecting to the server ....." + client.PortNr;
        //            ClientVmRef.AddToBuffer(client.ClientStatus);

        //            await streamSocket.ConnectAsync(hostName, client.PortNr);

        //            ClientVmRef.AddToBuffer("Client Connected");

        //            using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
        //            {
        //                using (var streamWriter = new StreamWriter(outputStream))
        //                {
        //                    await streamWriter.WriteLineAsync(client.SentMessage);
        //                    await streamWriter.FlushAsync();
        //                }
        //                ClientVmRef.AddToBuffer(String.Format("send message: \"{0}\"", client.SentMessage));

        //                using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
        //                {
        //                    using (var streamReader = new StreamReader(inputStream))
        //                    {
        //                        client.ReceivedMessage = await streamReader.ReadLineAsync();
        //                    }
        //                }
        //                ClientVmRef.AddToBuffer(string.Format("Received message: \"{0}\"", client.ReceivedMessage));
        //            }


        //            ClientVmRef.AddToBuffer("Client closed its socket");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
        //        client.ClientStatus = webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message;
        //        ClientVmRef.AddToBuffer(client.ClientStatus);
        //    }

        //}
    }
}
