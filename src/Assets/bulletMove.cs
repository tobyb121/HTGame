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
            Destroy(gameObject);
        }

        previousPosition = newPosition;
	}

    public Vector3 previousPosition;

    public Vector3 velocity;

    public LayerMask layerMask;

    private LineRenderer lineRenderer;
}