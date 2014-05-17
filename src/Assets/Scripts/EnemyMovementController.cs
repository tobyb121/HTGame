using UnityEngine;
using System.Collections;

public class EnemyMovementController : MonoBehaviour {

    public enemyController controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = controller.character.Position;
	}
}
