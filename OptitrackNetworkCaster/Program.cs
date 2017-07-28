using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;

namespace OptitrackNetworkCaster
{
    class Program
    {

        private static RecordingData mData;

        private static Byte[] mReadBytes;

        private static string mIP = "239.255.42.99", mFileName;

        private static int mPort = 1511, mWaitingTime = 20;

        static void Main(string[] args)
        {
            if (!CheckArguments(args))
            {
                if (args.Length == 0 || (args.Length == 1 && args[0] != "--help"))
                {
                    Console.WriteLine("Type \"--help\" if you need it!");
                }

                return;
            }

            if (!ReadRecordFile(args[0]))
            {
                return;
            }

            UdpClient client = new UdpClient();
            client.Client.ExclusiveAddressUse = false;

            IPAddress ip = IPAddress.Parse(mIP);
            client.JoinMulticastGroup(ip);
            IPEndPoint endPoint = new IPEndPoint(ip, mPort);

            Console.WriteLine("Streaming {0} to {1}:{2}!", mFileName, mIP, mPort);

            var t = new Thread(() =>
            {
                while (true)
                {
                    for (int i = 0; i < mData.GetFrameCount(); i++)
                    {
                        var currentFrame = mData.GetFrame(i);

                        var bytes = ObjectToProtoByteArray(currentFrame);

                        client.Send(bytes, bytes.Length, endPoint);

                        Thread.Sleep(mWaitingTime);

                        //var test = new MemoryStream(bytes);
                        //var testframe = Serializer.Deserialize<TimeFrame>(test);

                        if (i >= mData.GetFrameCount())
                        {
                            i = 0;
                        }
                    }
                }
            });

            t.Start();
        }

        private static bool CheckArguments(string[] args)
        {
            if (args.Length == 1 && args[0] == "--help")
            {
                Console.WriteLine("App to stream .mrc files over network. Takes arguments: File path; Optional UDP Broadcast IP; Optional Port; Optional Speed (Waiting time between broadcasts; lower value = faster broadcast)");
                return false;
            }

            try
            {
                FileInfo filePath = new FileInfo(args[0]);
                mFileName = filePath.Name;
            }
            catch (Exception)
            {

                Console.WriteLine("Invalid Path!");
                return false;
            }

            Regex ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            if (args.Length >= 2)
            {
                if (!ipRegex.IsMatch(args[1]))
                {
                    Console.WriteLine("Invalid IP adress. Using default one: " + mIP);
                }
                else
                {
                    mIP = args[1];
                }
            }

            int parsedPort = 0, parsedTime = 0;

            if (args.Length >= 3 && !Int32.TryParse(args[2], out parsedPort))
            {
                Console.WriteLine("Invalid Port. Usind default one: " + mPort);
            }
            else if (parsedPort != 0)
            {
                mPort = parsedPort;
            }

            if (args.Length >= 4 && !Int32.TryParse(args[3], out parsedTime))
            {
                Console.WriteLine("Invalid speed value. Usind default one: " + mWaitingTime);
            }
            else if (parsedTime != 0)
            {
                mWaitingTime = parsedTime;
            }

            return true;
        }

        private static bool ReadRecordFile(string path)
        {
            try
            {
                FileStream recordStream = File.OpenRead(path);
                //mReadBytes = File.ReadAllBytes(path);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Binder = new PreMergeToMergedDeserializationBinder();

                mData = binaryFormatter.Deserialize(recordStream) as RecordingData;

                recordStream.Close();

                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not read file " + path);
            }

            return false;
        }

        private static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private static byte[] ObjectToProtoByteArray(Object obj)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, obj);

            return ms.ToArray();
        }
    }

    sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName == "RecordingData")
            {
                return typeof(RecordingData);
            }
            if (typeName == "TimeFrame")
            {
                return typeof(TimeFrame);
            }
            if (typeName == "MotiveMarker")
            {
                return typeof(MotiveMarker);
            }
            if (typeName == "MotiveRigidbody")
            {
                return typeof(MotiveRigidbody);
            }

            if (typeName.Contains("Generic.List"))
            {
                if (typeName.Contains("TimeFrame"))
                {
                    return typeof(List<TimeFrame>);
                }
                if (typeName.Contains("MotiveMarker"))
                {
                    return typeof(List<MotiveMarker>);
                }
                if (typeName.Contains("MotiveRigidbody"))
                {
                    return typeof(List<MotiveRigidbody>);
                }
            }

            var type = Type.GetType(typeName);

            return type;
        }
    }
}
