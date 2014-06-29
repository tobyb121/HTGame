using UnityEngine;
using System.Collections;
using BloodyMuns;

public class characterProperties : MonoBehaviour {

	public Character character = new Character();
    public int CharacterId;
    public int CharacterHealth;
    
    private const int MaxHealth=100;
	// Use this for initialization
	void Start () {
        character.CharacterID = CharacterId;
        CharacterHealth = MaxHealth;
        Globals.Character = this;
	}
	
	// Update is called once per frame
	void Update () {
        character.Velocity = (transform.position-character.Position)/Time.deltaTime;
		character.Position = transform.position;
        character.Rotation = transform.rotation;

	}

    // I made this function for Derrick because he is my friend
    public void BulletHit()
    {
        CharacterHealth = CharacterHealth - 10;
        if (CharacterHealth == 0)
        {
            UnityEngine.Vector3 CurrentPosition = transform.position;
            CurrentPosition.y = 100;
            transform.position = CurrentPosition;

            CharacterHealth = MaxHealth;
        }
    }
}
