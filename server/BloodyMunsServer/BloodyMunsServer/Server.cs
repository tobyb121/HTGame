using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

using BloodyMuns;

namespace BloodyMunsServer
{
    class Server
    {
        public static ushort BC_PORT =Properties.Settings.Default.BC_PORT;
        public static ushort TCP_PORT=Properties.Settings.Default.TCP_PORT;
        public static ushort UDP_PORT=Properties.Settings.Default.UDP_PORT;

        private Socket tcpListener;

        public Socket udpListener;

        private Socket BCSocket;

        private List<Client> clients;
        
        private bool listening;
        public bool Listening
        {
            get { return listening; }
        }

        Thread clientUpdateThread;

        private Timer heartbeatThread;

        private Timer BCThread;

        private byte[] bcPacket;

        private Timer gameUpdateThread;

        public Server()
        {
            clients = new List<Client>(10);   
        }
        
        public void Start()
        {
            tcpListener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint listenEP = new IPEndPoint(IPAddress.Any, TCP_PORT);
            tcpListener.Bind(listenEP);
            tcpListener.Listen(50);
            listening = true;

            
            udpListener = new Socket(SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint udpEP = new IPEndPoint(IPAddress.Any, UDP_PORT);
            udpListener.Bind(udpEP);

            BCSocket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
            BCSocket.EnableBroadcast = true;
            
            heartbeatThread = new Timer(new TimerCallback(heartbeat));
            heartbeatThread.Change(1000, 1000);

            gameUpdateThread = new Timer(new TimerCallback(gameUpdate));
            //gameUpdateThread.Change(0, Properties.Settings.Default.UpdateTime);

            Console.WriteLine("Starting Listening on:" + ((IPEndPoint)tcpListener.LocalEndPoint).Address.ToString() + ":" + listenEP.Port.ToString());

            BCThread = new Timer(new TimerCallback(broadcastIP));
            BCThread.Change(500, 2500);

            clientUpdateThread = new Thread(new ThreadStart(clientUpdates));

            while (listening)
            {
                Socket clientSocket;
                try
                {
                    clientSocket = tcpListener.Accept();
                }
                catch (ObjectDisposedException) { break; }
                if (clientSocket != null && clientSocket.Connected)
                {
                    Client c = new Client(clientSocket);
                    clients.Add(c);
                }
            }
        }

        private byte[] setupBCPacket()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(memStream);
            bw.Write(TCP_PORT);
            bw.Write(UDP_PORT);
            bw.Write(Properties.Settings.Default.SERVER_NAME);
            bcPacket= memStream.ToArray();
            return bcPacket;
        }

        private void broadcastIP(object state)
        {
            IPEndPoint bc=new IPEndPoint(IPAddress.Broadcast, BC_PORT);
            BCSocket.SendTo(bcPacket??setupBCPacket(), bc);
        }

        private void heartbeat(object state)
        {
            if (clients.Count == 0)
                return;
            for (int i = clients.Count-1; i >= 0; i--) 
            {
                if (clients[i].Connected)
                    clients[i].ping();
                else
                    clients.RemoveAt(i);
            }
        }

        private void clientUpdates()
        {
            while (listening)
            {
                byte[] packet=new byte[256];
                EndPoint ep=new IPEndPoint(IPAddress.Any,0);
                int packetLength=udpListener.ReceiveFrom(packet,ref ep);
                Client client=clients.First(c => c.RemoteIP.Equals(((IPEndPoint)ep).Address));
                if (client!=null)
                    client.onClientUpdate(packet,packetLength);
            }
        }

        private void gameUpdate(object state)
        {
            MemoryStream outputStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(outputStream);
            bw.Write((byte)clients.Count);
            foreach (Client c in clients)
            {
                c.Character.writeCharacter(outputStream);
            }
            foreach (Client c in clients)
            {
                
                SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
                evt.SetBuffer(outputStream.GetBuffer(),0,(int)outputStream.Position);
                evt.RemoteEndPoint = new IPEndPoint(c.RemoteIP, UDP_PORT);
                udpListener.SendToAsync(evt);
            }
        }

        public void Stop()
        {
            tcpListener.Shutdown(SocketShutdown.Both);
            tcpListener.Close();
        }

    }
}
