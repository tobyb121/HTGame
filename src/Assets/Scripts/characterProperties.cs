using UnityEngine;
using System.Collections;
using BloodyMuns;

public class characterProperties : MonoBehaviour {

	public Character character = new Character();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		character.Position = transform.position;
		character.Rotation = transform.rotation;
		character.Velocity = rigidbody.velocity;
	}
}
