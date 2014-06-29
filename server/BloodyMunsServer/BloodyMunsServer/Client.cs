using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

using BloodyMuns;

namespace BloodyMunsServer
{
    class Client
    {
        private Socket tcpConnection;

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

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

            MemoryStream initPacketStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(initPacketStream);
            bw.Write((byte)0x70);
            bw.Write((byte)0x10);
            bw.Write(character.ID);

            tcpConnection.Send(initPacketStream.ToArray());
            StateObject state = new StateObject(tcpConnection);
            tcpConnection.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(onDataRx), state);
        }

        private bool connected;
        public bool Connected
        {
            get { return connected; }
        }

        private int p = 5;

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
                p--;
            }
        }

        private void onDataRx(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                StateObject state = (StateObject)result.AsyncState;
                int bytesRead = 0;
                try
                {
                    bytesRead = state.socket.EndReceive(result);
                }
                catch (SocketException)
                {
                    connected = false;
                    Console.WriteLine("Client Lost: "+name);
                    return;
                }
                catch (ObjectDisposedException)
                {
                    connected = false;
                    Console.WriteLine("Client Lost: "+name);
                    return;
                }
                switch (state.buffer[1])
                {
                    case 0xFF:
                        connected = true;
                        p = 5;
                        break;
                    case 0x20:
                        MemoryStream memStream = new MemoryStream(state.buffer, 2, bytesRead - 2);
                        handleClientUpdate(memStream);
                        break;
                }
                StateObject o=new StateObject(tcpConnection);
                try
                {
                    tcpConnection.BeginReceive(o.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(onDataRx), o);
                }
                catch { }
            }
        }

        public void onClientUpdate(Character c)
        {
            character = c;
        }

        public void handleClientUpdate(MemoryStream memStream)
        {
            BinaryReader br = new BinaryReader(memStream);
            name=br.ReadString();
            Console.WriteLine("Player has assumed the name: " + name);
        }

        public void close()
        {
            tcpConnection.Shutdown(SocketShutdown.Both);
            tcpConnection.Close();
            connected = false;
        }
        
        public class StateObject
        {
            public StateObject(Socket socket)
            {
                this.socket = socket;
            }
            public Socket socket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
        }
    }
}
