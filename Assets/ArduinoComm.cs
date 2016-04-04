using UnityEngine;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Threading;


public class ArduinoComm : MonoBehaviour
{
    private static bool _continue;
    private SerialPort comPort;
    private string[] comPortNames;
    private string message;
    Vector3 offsetRotation;
    bool bfirstRotation = false;
    Thread readThread;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        readThread = new Thread(Read);
    }

    void Start()
    {
        getPortNames();
        Connect();

    }

    void OnDestroy()
    {
        _continue = false;
        print("Shit");
    }

    void getPortNames()
    {
        comPortNames = SerialPort.GetPortNames();
    }

    /// <summary>
    /// Start connecting and sending the speed to the fan
    /// </summary>
    public void Connect()
    {
        if (comPortNames.Length > 0)
        {
            comPort = new SerialPort(comPortNames[comPortNames.Length - 1], 9600);
            comPort.ReadTimeout = 100;
            comPort.NewLine = "\n";
            comPort.Open();
            print("connect to " + comPortNames[comPortNames.Length - 1]);
            //   connected = true;
            _continue = true;
            readThread.Start();

        }
    }

    void Update()
    {
        if (message != null)
        {
           // print(message);
            Parse();
        }
    }

    public void Read()
    {
        while (_continue)
        {
            try
            {

                message = comPort.ReadLine();
            }
            catch { }
        }
    }

    private void Parse()
    {
        var r = new Regex(@"[0-9]+\.[0-9]+");
        //string[] numbers = Regex.Split(message, @"\D+");
        var mc = r.Matches(message);
        var matches = new Match[mc.Count];
        mc.CopyTo(matches, 0);
        var myFloats = new float[matches.Length];
        var ndx = 0;
        foreach (Match m in matches)
        {
            myFloats[ndx] = float.Parse(m.Value);
            ndx++;
        }
        if (myFloats.Length == 2)
        {
            print(message + "::::::::" + myFloats[0] + " " + myFloats[1] );
            if (!bfirstRotation)
            {
                offsetRotation = new Vector3(-myFloats[0], myFloats[1], 0);
                bfirstRotation = true;
            }
            transform.rotation = Quaternion.Euler(new Vector3(-myFloats[0], myFloats[1], 0) - offsetRotation);
        }
        else
        {
            print(message + "::::::::" + myFloats.Length);
        }
    }
}
