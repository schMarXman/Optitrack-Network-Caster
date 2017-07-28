using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using UnityEngine;

namespace OptitrackNetworkCaster
{
    [System.Serializable, ProtoContract]
    public class MotiveRigidbody
    {
        [ProtoMember(1)]
        public float posX;
        [ProtoMember(2)]
        public float posY;
        [ProtoMember(3)]
        public float posZ;
        [ProtoMember(4)]
        public float rotX;
        [ProtoMember(5)]
        public float rotY;
        [ProtoMember(6)]
        public float rotZ;
        [ProtoMember(7)]
        public float rotW;

        [ProtoMember(8)]
        public int id;

        public MotiveRigidbody(Vector3 pos, Quaternion rot, int id)
        {
            posX = pos.x;
            posY = pos.y;
            posZ = pos.z;

            rotX = rot.x;
            rotY = rot.y;
            rotZ = rot.z;
            rotW = rot.w;

            this.id = id;
        }

        public MotiveRigidbody() { }
    }
}
