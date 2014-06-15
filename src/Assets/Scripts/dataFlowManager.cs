using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using BloodyMuns;
using System.IO;
using System.Collections.Generic;


public class dataFlowManager : MonoBehaviour
{
	static UdpClient sBroadcast;
	IPAddress hostIP;
	ushort tcpPort;
	ushort udpPort;
    ushort clientUdpPort;
	IPEndPoint udpEP;
	static Socket tcp;
	static Socket udp;
	string serverName;
	public string playerName;
	public int bcPort = 5433;
	public characterProperties characterProperties;
	public bool connected;

    bool updateQueued = true;

    public GameObject[] EnemyPrefab;
    List<enemyController> Enemies = new List<enemyController>();
    static Thread thread;
    static Thread udpUpdatesThread;

	// Use this for initialization
	void Start ()
	{
        if (thread != null)
            thread.Abort();
        if (udpUpdatesThread != null)
            udpUpdatesThread.Abort();
		thread = new Thread (new ThreadStart (threadStart));
		thread.Start ();
        UnityEditor.EditorApplication.playmodeStateChanged+=new UnityEditor.EditorApplication.CallbackFunction(Close);
	}

	// Update is called once per frame
	void Update ()
	{
		if (connected&&updateQueued) {
			MemoryStream sendStream=new MemoryStream();
			characterProperties.character.writeCharacter(sendStream);
            udp.SendTo(sendStream.ToArray(),udpEP);
            print(characterProperties.character.CharacterID);
            updateQueued = false;
		}
        foreach (enemyController enemy in Enemies.FindAll(e=>e.enemy==null))
        {
            GameObject g = (GameObject)GameObject.Instantiate(EnemyPrefab[enemy.character.CharacterID]);
            g.GetComponent<EnemyMovementController>().controller = enemy;
            enemy.enemy = g;
        }
	}

    void udpUpdatesThreadStart()
    {
        byte[] buffer=new byte[4096];
        int failed = 0;
        while (true)
        {
            try
            {
                int bytesRx;
                try
                {
                    bytesRx = udp.Receive(buffer);
                }
                catch
                {
                    print("UDP Receive Error/Timeout");
                    failed++;
                    if (failed >= 5)
                        return;
                    continue;
                }
                failed = 0;
                MemoryStream memStream = new MemoryStream(buffer);
                BinaryReader reader = new BinaryReader(memStream);
                int numClients = reader.ReadByte();
                for (int i = 0; i < numClients; i++)
                {
                    Character c = Character.readCharacter(memStream);
                    if (c.ID != characterProperties.character.ID)
                    {
                        if (Enemies.Exists(x => x.character.ID == c.ID))
                        {
                            Enemies.Find(x => x.character.ID == c.ID).updateCharacter(c);
                        }
                        else
                        {
                            Enemies.Add(new enemyController(c));
                        }
                    }
                }
                updateQueued = true;
            }
            catch (System.Exception e)
            {
                print(e);
            }
        }
    }

	void threadStart ()
	{
		byte[] buffer = new byte[1024];
		byte[] heartBeatResponse = new byte[] {0x02,0xFF};

		IPEndPoint bcAddress = captureBcAddress (bcPort);
        
        print("Got bcAddress: " + bcAddress.ToString());
		tcp = initialiseTCP (bcAddress);
        if (tcp == null)
        {
            print("Error Connecting to TCP");
            return;
        }
        print("TCP initialised");
		udp = initialiseUDP ();

		int n=0;
        int failed = 0;
		while (true) {
            int received;
            try
            {
                received = tcp.Receive(buffer);
            }
            catch
            {
                print("TCP Receive Error/Timeout");
                failed++;
                if (!tcp.Connected || failed >= 5)
                {
                    tcp.Close();
                    return;
                }
                continue;
            }
            failed = 0;
			switch (buffer [1]) {
			case 0xFE:
				tcp.Send (heartBeatResponse);
				print ("Ping");
				break;

			case 0x10:
				//initialse character ID
				MemoryStream receivedStream = new MemoryStream(buffer,2,buffer.Length-2);
				BinaryReader receivedBinary = new BinaryReader(receivedStream);
				int charID = receivedBinary.ReadInt32();
				characterProperties.character.ID=charID;

				MemoryStream sendStream = new MemoryStream();
				BinaryWriter sendBinary = new BinaryWriter(sendStream);
				sendBinary.Write((byte)0x02);
				sendBinary.Write((byte)0x20);
				sendBinary.Write (playerName);
				tcp.Send(sendStream.ToArray());
				connected=true;
				break;

			case 0x11:
				//new character
				break;
		
			case 0x12:
				//character update
				break;
			}
				
		}
	}

	IPEndPoint captureBcAddress (int bcPort)
	{
        if (sBroadcast != null)
        {
            sBroadcast.Close();
        }
		sBroadcast = new UdpClient (bcPort);
        sBroadcast.Client.ReceiveTimeout = 5000;
        
		IPEndPoint capture = new IPEndPoint (IPAddress.Any, 0);
		EndPoint captureRemote = (EndPoint)capture;
        byte[] data=null;
        int x=0;
        while(data==null&&x<5){
            try
            {
                data = sBroadcast.Receive(ref capture);
            }
            catch {
                print("BC Receive Timeout: " + x);
            };
            x++;
        }
        if (data == null)
        {
            print("BC Receive Timed Out: No server found");
            sBroadcast.Close();
            return null;
        }

		MemoryStream receivedStream = new MemoryStream (data);
		BinaryReader receivedBinary = new BinaryReader (receivedStream);
		//byte[] bcByte = sBroadcast.Receive (ref chapture);
        print("Received Broadcast Message");
		hostIP = capture.Address;
        tcpPort = receivedBinary.ReadUInt16();
        udpPort = receivedBinary.ReadUInt16();
        clientUdpPort = receivedBinary.ReadUInt16();
        serverName = receivedBinary.ReadString();
        
		return new IPEndPoint (hostIP, tcpPort);
	}

	Socket initialiseTCP (IPEndPoint bcAddress)
	{
        if (tcp != null)
        {
            tcp.Shutdown(SocketShutdown.Both);
        }
        try
        {
            tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcp.Connect(bcAddress);
            tcp.ReceiveTimeout = 5000;
        }
        catch
        {
            tcp = null;
        }
		return tcp;
	}

	Socket initialiseUDP ()
	{
        if (udp != null)
        {
            udp.Shutdown(SocketShutdown.Both);
        }
		Socket sUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		sUDP.Bind(new IPEndPoint(IPAddress.Any,clientUdpPort));
        udpEP = new IPEndPoint(hostIP, udpPort);
        sUDP.ReceiveTimeout = 5000;
        
        udpUpdatesThread = new Thread(new ThreadStart(udpUpdatesThreadStart));
        udpUpdatesThread.Start();
		return sUDP;
	}


    void Close()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            if(udpUpdatesThread!=null)
                udpUpdatesThread.Abort();
            if(thread!=null)
                thread.Abort();
        }
    }
}

