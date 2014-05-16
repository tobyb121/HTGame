using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using BloodyMuns;
using System.IO;

public class dataFlowManager : MonoBehaviour
{
	UdpClient sBroadcast;
	IPAddress tcpHost;
	ushort tcpPort;
	ushort udpPort;
	Socket tcp;
	Socket udp;
	string serverName;
	string playerName="Mugabe";
	public int bcPort = 5433;
	public characterProperties characterProperties;
	public bool connected;

	// Use this for initialization
	void Start ()
	{
		Thread thread = new Thread (new ThreadStart (threadStart));
		thread.Start ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (connected) {
			MemoryStream sendStream=new MemoryStream();
			characterProperties.character.writeCharacter(sendStream);
			udp.Send(sendStream);
		}

	}
	
	void threadStart ()
	{
		byte[] buffer = new byte[1024];
		byte[] heartBeatResponse = new byte[] {0x02,0xFF};

		IPEndPoint bcAddress = captureBcAddress (bcPort);
		tcp = initialiseTCP (bcAddress);
		udp = initialiseUDP ();

		int n=0;
		while (true) {
			int received = tcp.Receive (buffer);
			print(n++);

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
		sBroadcast = new UdpClient (bcPort);

		IPEndPoint capture = new IPEndPoint (IPAddress.Any, 0);
		EndPoint captureRemote = (EndPoint)capture;

		MemoryStream receivedStream = new MemoryStream (sBroadcast.Receive (ref capture));
		BinaryReader receivedBinary = new BinaryReader (receivedStream);
		//byte[] bcByte = sBroadcast.Receive (ref capture);

		tcpHost = capture.Address;

		tcpPort = receivedBinary.ReadUInt16 ();
		udpPort = receivedBinary.ReadUInt16 ();
		serverName = receivedBinary.ReadString ();

		return new IPEndPoint (tcpHost, tcpPort);
	}

	Socket initialiseTCP (IPEndPoint bcAddress)
	{
		Socket tcp = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		tcp.Connect (bcAddress);
		return tcp;
	}

	Socket initialiseUDP ()
	{
		Socket sUDP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
		sUDP.Bind(new IPEndPoint(IPAddress.Any,udpPort));
		return sUDP;
	}

}

