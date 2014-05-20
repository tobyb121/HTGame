using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {
    void Start () {
        rigidbody.velocity = 200 * transform.forward;
        previousPosition = rigidbody.position;
	}
	
	// Update is called once per frame
	void Update () {
	}

    private Vector3 previousPosition;

    public LayerMask layerMask;

    void FixedUpdate()
    {
        //Vector3 movementThisStep =rigidbody.position - previousPosition;
        //float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        //if (movementSqrMagnitude > sqrMinimumExtent)
        //{
        //    float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
        //    RaycastHit hitInfo;

        //    //check for obstructions we might have missed 
        //    if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
        //        myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
        //}

        //previousPosition = myRigidbody.position;
    }

}