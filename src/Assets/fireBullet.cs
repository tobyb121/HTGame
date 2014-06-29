using UnityEngine;
using System.Collections;
using System.IO;

public class fireBullet : MonoBehaviour {

    public Transform forward;

    public float bulletSpeed = 1000f;

	void Start () {
	
	}

	
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(0x12);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(forward.position.x);
            bw.Write(forward.position.y);
            bw.Write(forward.position.z);
            bw.Write(forward.forward.x * bulletSpeed);
            bw.Write(forward.forward.y * bulletSpeed);
            bw.Write(forward.forward.z * bulletSpeed);
            Globals.network.sendMessage(ms.ToArray());
        }
	}
}
