using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BloodyMunsServer
{
    [Serializable]
    public struct Vector3
    {
        int x;
        int y;
        int z;
        public Vector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public struct Quaternion
    {
        int x;
        int y;
        int z;
        int w;
        public Quaternion(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
