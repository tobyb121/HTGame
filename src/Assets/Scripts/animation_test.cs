using UnityEngine;
using System.Collections;

public class animation_test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.DownArrow))
			GetComponent<Animator> ().SetFloat ("speed", 1);
		else
			GetComponent<Animator> ().SetFloat ("speed", 0);

	}
}
