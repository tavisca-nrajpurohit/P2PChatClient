using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    class ChatClient
    {
        String _myName;
        int _myPort;
        String _clientName = "Client";
        int _clientPort;
        String _clientIP;
        IPHostEntry _ipHost;
        IPAddress _ipAddr;

        public ChatClient()
        {
            _ipHost = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddr = _ipHost.AddressList[1];
        }

        public void RunNode()
        {
            Console.WriteLine("Your Ip Address : " + _ipAddr);
            Console.Write("Input your Port : ");
            this._myPort = Int32.Parse(Console.ReadLine());
            Console.Write("Input your Name :");
            this._myName = Console.ReadLine();

            Console.Write("Input Remote Host IP : ");
            this._clientIP = Console.ReadLine();
            Console.Write("Input Remote Host Port : ");
            this._clientPort = Int32.Parse(Console.ReadLine());

            Thread sendingThread = new Thread(new ThreadStart(() => this.Send()));
            sendingThread.Start();
            Thread listeningThread = new Thread(new ThreadStart(() => this.Listen()));
            listeningThread.Start();
        }

        public void Send()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_clientIP), _clientPort);

            try
            {
                var flag = true;
                while(flag)
                {
                    try
                    {
                        socket.Connect(endPoint);
                        flag = false;
                        Console.WriteLine("Connected!");
                    }
                    catch
                    {
                        Console.WriteLine("Waiting for connection");
                    }
                }
                
                while (true)
                {
                    var message = Console.ReadLine();
                    var messageToBeSent = Encoding.ASCII.GetBytes(message);
                    socket.Send(messageToBeSent);
                }
            }
            catch
            {
                Console.WriteLine("Error: Unable to connect to Server!");
            }
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();

        }

        public void Listen()
        {
            IPEndPoint endPoint = new IPEndPoint(_ipAddr, _myPort);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            listener.Listen(10);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket = listener.Accept();
                while (true)
                {
                    var messageRecieved = new byte[1024];
                    var byteRecieved = socket.Receive(messageRecieved);
                    Console.WriteLine(_clientName + ": " + Encoding.ASCII.GetString(messageRecieved, 0, byteRecieved));
                }            
            }
            catch
            {
                Console.WriteLine("Error: Unable to connect to Client!");
            }
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
            
        }
    }
}
