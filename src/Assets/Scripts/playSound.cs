using UnityEngine;
using System.Collections;

public class playSound : MonoBehaviour {

   void Start () {
       if (audio == null)
          Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (!audio.isPlaying)
            Destroy(gameObject);
	}
}
