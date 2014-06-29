using UnityEngine;
using System.Collections;

public class healthText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        guiText.text = "Health: " + Mathf.Round(Globals.Character.CharacterHealth);
	}
}
