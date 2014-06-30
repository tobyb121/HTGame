using UnityEngine;
using System.Collections;

public class bulletFireTest : MonoBehaviour {

    public Transform forward;
    public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = (GameObject)Instantiate(bulletPrefab);
            go.transform.position = forward.position;
            go.GetComponent<bulletMove>().previousPosition = forward.position;
            go.GetComponent<bulletMove>().velocity = forward.forward * 20;
        }
	    
	}
}
