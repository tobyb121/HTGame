﻿using System;
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
        public static ushort CLIENT_UDP_PORT = Properties.Settings.Default.CLIENT_UDP_PORT;

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
            gameUpdateThread.Change(0, Properties.Settings.Default.UpdateTime);

            Console.WriteLine("Starting Listening on:" + ((IPEndPoint)tcpListener.LocalEndPoint).Address.ToString() + ":" + listenEP.Port.ToString());

            BCThread = new Timer(new TimerCallback(broadcastIP));
            BCThread.Change(500, 2500);

            clientUpdateThread = new Thread(new ThreadStart(clientUpdates));
            clientUpdateThread.Start();

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
            bw.Write(CLIENT_UDP_PORT);
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
            byte[] packet = new byte[256];
            while (listening)
            {
                EndPoint ep=new IPEndPoint(IPAddress.Any,0);
                int packetLength;
                try
                {
                    packetLength = udpListener.ReceiveFrom(packet, ref ep);
                }
                catch (Exception e)
                {
                    continue;
                }

                MemoryStream memoryStream = new MemoryStream(packet,0,packetLength);
                int messageType = memoryStream.ReadByte();
                switch (messageType) { 
                    case 0x11:
                        Character character = Character.readCharacter(memoryStream);

                        Client client=clients.FirstOrDefault(c => c.Character.ID==character.ID);
                        if (client != null)
                            client.onClientUpdate(character);
                        break;
                    case 0x12:
                        for (int i = 0; i < clients.Count; i++)
                        {
                            packet[0] = 0x02;
                            SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
                            evt.SetBuffer(packet, 0, packet.Length);
                            evt.RemoteEndPoint = new IPEndPoint(clients[i].RemoteIP, CLIENT_UDP_PORT);
                            udpListener.SendToAsync(evt);
                        }
                        break;
                }
            }
        }

        private void gameUpdate(object state)
        {
            MemoryStream outputStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(outputStream);
            bw.Write((byte)0x01);
            bw.Write((byte)clients.Where(c=>c.Character.CharacterID!=-1).Count());
            foreach (Client c in clients.Where(c => c.Character.CharacterID != -1))
            {
                c.Character.writeCharacter(outputStream);
            }
            byte[] update = outputStream.ToArray();
            for (int i = 0; i < clients.Count; i++ )
            {
                SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
                evt.SetBuffer(update, 0, update.Length);
                evt.RemoteEndPoint = new IPEndPoint(clients[i].RemoteIP, CLIENT_UDP_PORT);
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
