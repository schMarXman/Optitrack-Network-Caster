using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace OptitrackNetworkCaster
{
    [System.Serializable, ProtoContract]
    public class RecordingData
    {
        [ProtoMember(1)]
        int count;
        [ProtoMember(2)]
        List<TimeFrame> frames = new List<TimeFrame>();

        public RecordingData() { }

        public void AddFrame(TimeFrame frame)
        {
            frame.count = frames.Count;
            frames.Add(frame);

            count++;
        }

        public TimeFrame GetFrame(int t)
        {
            return frames[t];
        }

        public int GetFrameCount()
        {
            return count;
        }

    }
}
