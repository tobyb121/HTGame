using UnityEngine;
using System.Collections;

public class splash : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public AudioClip starting;

    public Animation splats;

    public GameObject pressAnyKey;

    bool waiting = false;
	
	// Update is called once per frame
	void Update () {
        if (!splats.isPlaying&&!waiting)
            pressAnyKey.SetActive(true);
        if (!splats.isPlaying && Input.anyKeyDown&&!waiting)
        {            
            audio.Stop();
            audio.clip = starting;
            audio.loop = false;
            audio.Play();
            pressAnyKey.SetActive(false);
            waiting = true;
        }
        if (!audio.isPlaying)
        {
            Application.LoadLevel("MainMenu");
        }
	}
}
