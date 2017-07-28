using System;

[System.Serializable]
public class RecordingData
{
    int count;
    List<TimeFrame> frames = new List<TimeFrame>();

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
