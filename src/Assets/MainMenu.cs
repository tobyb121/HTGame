using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        Screen.showCursor = false;
        for (int i = 0; i < characters.Length; i++ )
        {
            float px=5*Mathf.Sin(2*i*Mathf.PI/characters.Length);
            float pz=5*Mathf.Cos(2*i*Mathf.PI/characters.Length);
            characters[i].transform.position = new Vector3(px, 1, pz);
        }
	}

    public GameObject[] characters;

    public string[] characterNames;

    public GUIText characterName;

	// Update is called once per frame
    void Update()
    {
        if (!Application.isLoadingLevel)
        {
            characterName.text = "";
            for (int i = 0; i < characters.Length; i++)
            {
                if (Vector3.Angle(transform.forward, characters[i].transform.position) < 15)
                {
                    characters[i].GetComponent<Animator>().SetFloat("speed", 5);
                    characterName.text = characterNames[i];
                    if (Input.GetMouseButtonDown(0))
                    {
                        Globals.selectedCharacter = i;
                        Application.LoadLevel("Map1");
                    }
                }
                else
                {
                    characters[i].GetComponent<Animator>().SetFloat("speed", 0);
                }
            }
        }
    }
}
