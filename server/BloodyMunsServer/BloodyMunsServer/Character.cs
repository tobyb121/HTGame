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
        public static int getID()
        {
            return _id++;
        }
        private static int _id = 0;

        public Character()
        {
            id = getID();
        }

        [DataMember]
        private int id;
        public int ID
        {
            get
            {
                return id;
            }
        }

        [DataMember]
        private int characterID;
        public int CharacterID
        {
            get
            {
                return characterID;
            }
        }

        [DataMember]
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        [DataMember]
        private Vector3 velocity;
        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
        }

        [DataMember]
        private Quaternion rotation;
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
        }

    }
}
