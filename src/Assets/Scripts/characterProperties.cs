using UnityEngine;
using System.Collections;
using BloodyMuns;

public class characterProperties : MonoBehaviour {

	public Character character = new Character();
    public int CharacterId;
    public float CharacterHealth;

    public int bullets = 20;
    public const int maxBullets = 30;

    public float recoveryRate = 10;

    public float lastHit = 5;
    
    public const float MaxHealth=100;
	// Use this for initialization
	void Start () {
        transform.position = new UnityEngine.Vector3(Random.Range(-45,45),50,Random.Range(-45,45));
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
        if (lastHit>2.5f)
        {
            CharacterHealth += recoveryRate * Time.deltaTime;
            if (CharacterHealth > MaxHealth)
                CharacterHealth = MaxHealth;
        }

        if (Input.GetKeyDown(KeyCode.R)&&bullets<maxBullets&&!audio.isPlaying)
            reload();

	}

    public void reload()
    {
        bullets = 0;
        audio.Play();
        StartCoroutine(waitForReload());
    }

    IEnumerator waitForReload()
    {
        while (audio.isPlaying)
        {
            yield return null;
        }
        bullets = maxBullets;
    }

    public void respawn()
    {
        UnityEngine.Vector3 CurrentPosition = transform.position;
        CurrentPosition.y = 100;
        transform.position = CurrentPosition;

        CharacterHealth = MaxHealth;

        bullets = maxBullets;
    }

    // I made this function for Derrick because he is my friend
    public void BulletHit()
    {
        lastHit = 0;
        CharacterHealth = CharacterHealth - 20;
        if (CharacterHealth <= 0)
        {
            respawn();
        }
    }
}
