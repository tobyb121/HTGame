using UnityEngine;
using System.Collections;
using BloodyMuns;

public class characterProperties : MonoBehaviour {

	public Character character = new Character();
    public int CharacterId;
	// Use this for initialization
	void Start () {
        character.CharacterID = CharacterId;
	}
	
	// Update is called once per frame
	void Update () {
        character.Velocity = (transform.position-character.Position)/Time.deltaTime;
		character.Position = transform.position;
        character.Rotation = transform.rotation;
	}
}
