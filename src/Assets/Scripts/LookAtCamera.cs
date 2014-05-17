using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
        renderers = GetComponentsInChildren<SpriteRenderer>();
	}

    private Camera mainCamera;

    public int depth = 0;
    SpriteRenderer[] renderers;

	// Update is called once per frame
	void Update () {
        Vector3 cameraDirection=transform.position-mainCamera.transform.position;
        cameraDirection.y= 0;
        Quaternion q = transform.rotation;
        q.SetLookRotation(cameraDirection);
        transform.rotation = q;
        foreach (SpriteRenderer s in renderers)
        {
            s.sortingOrder = depth * 100 + s.sortingOrder % 100;
        }
    }
}
