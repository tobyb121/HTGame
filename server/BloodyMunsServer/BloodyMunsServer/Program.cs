using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyMunsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server serverInstance=new Server();
            serverInstance.Start();
        }
    }
}
