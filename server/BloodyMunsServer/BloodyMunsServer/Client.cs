﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace BloodyMunsServer
{
    class Client
    {
        private Socket tcpConnection;

        private IPAddress remoteIP;
        public IPAddress RemoteIP
        {
            get
            {
                return remoteIP;
            }
        }

        public Client(Socket socket)
        {
            tcpConnection = socket;
            remoteIP = ((IPEndPoint) tcpConnection.RemoteEndPoint).Address;
            Console.WriteLine("New Client: " + remoteIP.ToString());
            connected = true;

            character = new Character();

            /*MemoryStream initPacketStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(initPacketStream);
            bw.Write(0x70);
            bw.Write(0x10);
            bw.Write(character.ID);

            tcpConnection.Send();*/
        }

        private bool connected;
        public bool Connected
        {
            get { return connected; }
        }

        private int p = 5;
        private bool pending = false;

        private Character character;
        public Character Character
        {
            get
            {
                return character;
            }
        }

        public void ping()
        {
            if (p == 0)
            {
                close();
                Console.WriteLine("Disconnecting Client: "+remoteIP.ToString()+" due to 5 missed pings");
                return;
            }
            if(tcpConnection.Connected){
                tcpConnection.Send(new byte[]{0x70,0xFE});
                StateObject state = new StateObject(tcpConnection);
                p--;
                if (!pending)
                {
                    tcpConnection.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(onPingRx), state);
                    pending = true;
                }
            }
        }

        private void onPingRx(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                StateObject state = (StateObject)result.AsyncState;
                int bytesRead = 0;
                try
                {
                    bytesRead = state.socket.EndReceive(result);
                }
                catch (SocketException e)
                {
                    connected = false;
                    Console.WriteLine("Client Lost");
                    return;
                }
                catch (ObjectDisposedException e)
                {
                    connected = false;
                    Console.WriteLine("Client Lost");
                    return;
                }
                if (bytesRead == 2 && (state.buffer[0] == 0x02) && (state.buffer[1] == 0xFF))
                {
                    connected = true;
                    pending = false;
                    p = 5;
                }
                else
                {
                    Console.WriteLine("Client Error: "+remoteIP.ToString()+" disconnected due to bad ping response");
                    connected = false;
                }
            }
        }

        public void update(byte[] data,int length)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Character));
            Character c = (Character)serializer.ReadObject(memoryStream);
        }

        public class StateObject
        {
            public StateObject(Socket socket)
            {
                this.socket = socket;
            }
            public Socket socket = null;
            public const int BufferSize = 128;
            public byte[] buffer = new byte[BufferSize];
        }


        public void close()
        {
            tcpConnection.Shutdown(SocketShutdown.Both);
            tcpConnection.Close();
            connected = false;
        }
    }
}
