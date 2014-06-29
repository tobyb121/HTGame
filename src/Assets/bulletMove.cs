using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
	}
	
	
	void Update () {
        Vector3 newPosition = previousPosition + velocity * Time.deltaTime;

        lineRenderer.SetPosition(0, previousPosition);
        lineRenderer.SetPosition(1, newPosition);

        RaycastHit hitInfo=new RaycastHit();
        if (Physics.Raycast(new Ray(previousPosition, velocity), out hitInfo, (newPosition - previousPosition).magnitude, layerMask))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                GameObject splat = (GameObject)Instantiate(bloodSplat);
                splat.transform.position = hitInfo.point;
            }
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Me"))
            {
                
            }
            Destroy(gameObject);
        }

        previousPosition = newPosition;
	}

    public GameObject bloodSplat;

    public Vector3 previousPosition;

    public Vector3 velocity;

    public LayerMask layerMask;

    private LineRenderer lineRenderer;
}