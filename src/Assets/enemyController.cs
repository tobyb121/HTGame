using UnityEngine;
using System.Collections;
using BloodyMuns;

public class enemyController : MonoBehaviour {

    public Character character;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = character.Position;
        transform.rotation = character.Rotation;
	}
}
