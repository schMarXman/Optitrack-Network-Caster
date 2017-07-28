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
    public class MotiveMarker
    {
        [ProtoMember(1)]
        public float posX;
        [ProtoMember(2)]
        public float posY;
        [ProtoMember(3)]
        public float posZ;

        public MotiveMarker(Vector3 pos)
        {
            posX = pos.x;
            posY = pos.y;
            posZ = pos.z;
        }

        public MotiveMarker() { }
    }
}
