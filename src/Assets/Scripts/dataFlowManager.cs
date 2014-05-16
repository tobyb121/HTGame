using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Net;

public class dataFlowManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Thread thread = new Thread (new ThreadStart (threadStart));
		thread.Start ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	UdpClient sBroadcast;
	IPAddress tcpHost;
	ushort tcpPort;
	public int bcPort=5433;

	void threadStart () {
		byte[] buffer = new byte[64];
		byte[] sendBuffer = new byte[] {0x02,0xFF};

		IPEndPoint bcAddress = captureBcAddress (bcPort);
		Socket s = initialiseTCP (bcAddress);
		int received = s.Receive(buffer);
		s.Send (sendBuffer);
	}


	IPEndPoint captureBcAddress(int bcPort) {
		sBroadcast = new UdpClient(bcPort);
		
		IPEndPoint capture = new IPEndPoint (IPAddress.Any, 0);
		EndPoint captureRemote = (EndPoint)capture;
		
		byte[] bcByte = sBroadcast.Receive (ref capture);
		
		tcpHost = capture.Address;
		
		tcpPort |= bcByte [0];
		tcpPort |= (ushort)(((ushort)bcByte [1]) << 8);

		return new IPEndPoint (tcpHost,tcpPort);
	}

	Socket initialiseTCP(IPEndPoint bcAddress) {
		Socket s = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		s.Connect(bcAddress);
		return s;
	}
	
}
