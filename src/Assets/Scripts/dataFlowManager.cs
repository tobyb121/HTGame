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

	Socket s;
	IPAddress HOST= new IPAddress( new byte[] {192,168,0,21});
	int PORT=5411;
	void threadStart () {
		byte[] buffer = new byte[64];
		s = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		s.Connect (HOST, PORT);

		byte[] sendBuffer = new byte[] {0x02,0xFF};
		while (true) {
			int received = s.Receive(buffer);
			s.Send (sendBuffer);
		}
	}
}
