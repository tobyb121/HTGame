using UnityEngine;
using System.Collections;

public class fireBullet : MonoBehaviour {

    public Transform forward;

	// Use this for initialization
	void Start () {
	
	}

    public GameObject bulletPrefab;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = (GameObject)GameObject.Instantiate(bulletPrefab);
            bullet.transform.rotation = forward.rotation;
            bullet.transform.position = forward.position;
        }
	}
}
