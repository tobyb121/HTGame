using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class muteMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
            audio.mute = !audio.mute;
	}
}
