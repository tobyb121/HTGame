using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {
    void Start () {
        GameObject sound = (GameObject)Instantiate(fireSound);
        sound.transform.position = previousPosition;

        lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.SetPosition(0, previousPosition);
        lineRenderer.SetPosition(1, previousPosition);

        Destroy(gameObject, 5);

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
                Destroy(splat, 1.0f);
            }
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Me"))
            {
                Globals.Character.BulletHit();
            }
            Destroy(gameObject);
        }

        previousPosition = newPosition;
	}

    public GameObject bloodSplat;

    public GameObject fireSound;

    public Vector3 previousPosition;

    public Vector3 velocity;

    public LayerMask layerMask;

    private LineRenderer lineRenderer;
}