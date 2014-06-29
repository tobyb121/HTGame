using UnityEngine;
using System.Collections;
using BloodyMuns;

public class characterProperties : MonoBehaviour {

	public Character character = new Character();
    public int CharacterId;
    public float CharacterHealth;

    public float recoveryRate = 10;

    public float lastHit = 5;
    
    public const float MaxHealth=100;
	// Use this for initialization
	void Start () {
        CharacterId = Globals.selectedCharacter;
        character.CharacterID = CharacterId;
        CharacterHealth = MaxHealth;
        Globals.Character = this;
	}
	
	// Update is called once per frame
	void Update () {
        character.Velocity = (transform.position-character.Position)/Time.deltaTime;
		character.Position = transform.position;
        character.Rotation = transform.rotation;

        lastHit += Time.deltaTime;
        if (lastHit>3)
        {
            CharacterHealth += recoveryRate * Time.deltaTime;
            if (CharacterHealth > MaxHealth)
                CharacterHealth = MaxHealth;
        }

	}

    // I made this function for Derrick because he is my friend
    public void BulletHit()
    {
        lastHit = 0;
        CharacterHealth = CharacterHealth - 10;
        if (CharacterHealth <= 0)
        {
            UnityEngine.Vector3 CurrentPosition = transform.position;
            CurrentPosition.y = 100;
            transform.position = CurrentPosition;

            CharacterHealth = MaxHealth;
        }
    }
}
