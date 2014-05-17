using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;
using System.IO;
using BloodyMuns;

namespace BloodyMunsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Character c = new Character();
            MemoryStream stream = new MemoryStream();
            BinaryWriter streamWriter = new BinaryWriter(stream);
            streamWriter.Write(1);
            c.writeCharacter(stream);
            byte[] bytes=stream.ToArray();
            MemoryStream readStream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(readStream);
            int n=reader.ReadInt32();
            Character.readCharacter(readStream);

            Server serverInstance=new Server();
            serverInstance.Start();
        }
    }
}
