using UnityEngine;
using System.Collections;

public class AmmoText : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        guiText.text = new string('#', Globals.Character.bullets) + new string('!', characterProperties.maxBullets - Globals.Character.bullets);
	}
}
