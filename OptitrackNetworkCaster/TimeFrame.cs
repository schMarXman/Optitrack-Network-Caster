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
    public class TimeFrame
    {
        [ProtoMember(1)]
        public int count;
        [ProtoMember(2)]
        public List<MotiveMarker> motiveMarkers = new List<MotiveMarker>();
        [ProtoMember(3)]
        public List<MotiveRigidbody> motiveRigidbodies = new List<MotiveRigidbody>();

        public void AddMarker(Vector3 position)
        {
            motiveMarkers.Add(new MotiveMarker(position));
        }

        public void AddRigidbody(Vector3 position, Quaternion rotation, int id)
        {
            motiveRigidbodies.Add(new MotiveRigidbody(position, rotation, id));
        }

        public TimeFrame() { }
    }
}
