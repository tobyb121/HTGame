using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace BloodyMunsServer
{
    [DataContract]
    class Character
    {
        [DataMember]
        private int id=1;
        public int ID
        {
            get
            {
                return id;
            }
        }

        [DataMember]
        private int characterID=2;
        public int CharacterID
        {
            get
            {
                return characterID;
            }
        }

        [DataMember]
        private Vector3 position=new Vector3(4,5,6);
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        [DataMember]
        private Quaternion rotation=new Quaternion(7,8,9,10);
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
        }

    }
}
