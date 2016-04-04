using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// State object for reading client data asynchronously
public class StateObject
{
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

[Serializable]
public class SocketData
{
    public bool bSpawn;
    // Data To Send
    public int zrotForce = 4;
    public int MaxRot = 90;
    public int MinRot = -90;
    public int rotupForce = 1;
    public float speed = 50;
    public float speedincrease = 4;
    public float speeddecrease = 1;
    public int Maxspeed = 100;
    public int Minspeed = 0;
    public int takeoffspeed = 20;
    public int lift = 3;
    public int minlift = 0;
    public bool hit = false;
    public int SpawnPositionIndex = 0;
    public float Elevation = 170;
}

public class GameController : MonoBehaviour
{

    private static bool bSpawn = false;
    private static SocketData PlaneData;
    private static bool _ContinuePinging;
    [Space(20)]
    public Transform[] SpawnLocations;
    public GameObject PlanePrefab;
    // Use this for initialization
    Thread ClientThread;
    void Start()
    {
        _ContinuePinging = true;
        ClientThread = new Thread(StartClient);
        ClientThread.Start();
    }

    void Update()
    {
        if (bSpawn)
        {
            SpawnPlane();
            bSpawn = false;
        }
    }

    void SpawnPlane()
    {
        if (SpawnLocations[PlaneData.SpawnPositionIndex] && PlanePrefab)
        {
            Vector3 SpawnPosition = SpawnLocations[PlaneData.SpawnPositionIndex].position;
            SpawnPosition.y = PlaneData.Elevation;
            GameObject PlaneTrans = Instantiate(PlanePrefab, SpawnPosition, SpawnLocations[PlaneData.SpawnPositionIndex].rotation) as GameObject;
            
            Plane MyPlane = PlaneTrans.GetComponent<Plane>();
            LoadPlaneData(MyPlane);
        }
    }

    void LoadPlaneData(Plane MyPlane)
    {
        MyPlane.zrotForce = PlaneData.zrotForce;
        MyPlane.MaxRot = PlaneData.MaxRot;
        MyPlane.MinRot = PlaneData.MinRot;
        MyPlane.rotupForce = PlaneData.rotupForce;
        MyPlane.speed = PlaneData.speed;
        MyPlane.speedincrease = PlaneData.speedincrease;
        MyPlane.speeddecrease = PlaneData.speeddecrease;
        MyPlane.Maxspeed = PlaneData.Maxspeed;
        MyPlane.Minspeed = PlaneData.Minspeed;
        MyPlane.takeoffspeed = PlaneData.takeoffspeed;
        MyPlane.lift = PlaneData.lift;
        MyPlane.minlift = PlaneData.minlift;
        MyPlane.hit = PlaneData.hit;
    }

    public static void StartClient()
    {
        // Data buffer for incoming data.
        byte[] bytes = new byte[1024];

        // Connect to a remote device.
        try
        {
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 12345);
            while (_ContinuePinging)
            {
                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    print("Socket connected to " +
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");


                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);

                    PlaneData = DeSerializeObject(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    if (PlaneData.bSpawn)
                    {
                        print("Spawn Command Given");
                        bSpawn = true;
                    }
                    else
                    {
                        print("No Spawn Command Given");

                    }

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    print("ArgumentNullException : " + ane.ToString());
                }
                catch (SocketException se)
                {
                    print("SocketException : " + se.ToString());
                }
                catch (Exception e)
                {
                    print("Unexpected exception : " + e.ToString());
                }
            }
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    void OnDestroy()
    {
        _ContinuePinging = false;
    }

    public static string SerializeObject(SocketData toSerialize)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

        using (MemoryStream ms = new MemoryStream())
        {
            xmlSerializer.Serialize(ms, toSerialize);
            return Convert.ToBase64String(ms.ToArray());
        }
    }

    public static SocketData DeSerializeObject(string toDeserialize)
    {
        SocketData Data = new SocketData();
        XmlSerializer xmlSerializer = new XmlSerializer(Data.GetType());
        byte[] bytes = Convert.FromBase64String(toDeserialize);

        using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
        {
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            Data = (SocketData)xmlSerializer.Deserialize(ms);
            return Data;
        }
    }

    public string ObjectToString(object obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            new BinaryFormatter().Serialize(ms, obj);
            return Convert.ToBase64String(ms.ToArray());
        }
    }

    public object StringToObject(string base64String)
    {
        byte[] bytes = Convert.FromBase64String(base64String);
        using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
        {
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            return new BinaryFormatter().Deserialize(ms);
        }
    }
}

