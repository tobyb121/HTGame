using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	}

    private Camera mainCamera;

	// Update is called once per frame
	void Update () {
        Vector3 cameraDirection=transform.position-mainCamera.transform.position;
        cameraDirection.y= 0;
        Quaternion q = transform.rotation;
        q.SetLookRotation(cameraDirection);
        transform.rotation = q;
    }
}
