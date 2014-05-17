using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.IO;

namespace BloodyMuns
{
    public class Character
    {
        public static int getID()
        {
            return _id++;
        }
        private static int _id = 0;


        private int id= getID();
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private int characterID;
        public int CharacterID
        {
            get
            {
                return characterID;
            }
            set
            {
                characterID=value;
            }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        private Vector3 velocity;
        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        private Quaternion rotation;
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public void update(Character source)
        {
            characterID = source.characterID;
            position = source.position;
            velocity = source.velocity;
            rotation = source.rotation;
        }

        public static Character readCharacter(MemoryStream mem)
        {
            Character result = new Character();

            BinaryReader reader = new BinaryReader(mem);

            result.id = reader.ReadInt32();
            result.characterID = reader.ReadInt32();
            result.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            result.velocity = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            result.rotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),reader.ReadSingle());

            return result;
        }

        public void writeCharacter(MemoryStream mem)
        {
            BinaryWriter writer = new BinaryWriter(mem);
            writer.Write(id);
            writer.Write(characterID);
            writer.Write(position.x);
            writer.Write(position.y);
            writer.Write(position.z);
            writer.Write(velocity.x);
            writer.Write(velocity.y);
            writer.Write(velocity.z);
            writer.Write(rotation.x);
            writer.Write(rotation.y);
            writer.Write(rotation.z);
            writer.Write(rotation.w);
        }

    }
}
