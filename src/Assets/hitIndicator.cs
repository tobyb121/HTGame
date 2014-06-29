using UnityEngine;
using System.Collections;

public class hitIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}

    float alpha = 0;

	// Update is called once per frame
	void Update () {
        if (Globals.Character.lastHit < 2 * Time.deltaTime)
        {
            alpha = 1;
        }
        else
        {
            float target = (characterProperties.MaxHealth - Globals.Character.CharacterHealth) / characterProperties.MaxHealth;
            alpha = alpha - 0.25f * (alpha - target);
        }
        guiTexture.pixelInset = new Rect(-Screen.width / 2-1, -Screen.height / 2-1, Screen.width+2, Screen.height+2);
        guiTexture.color = new Color(1, 1, 1, alpha);
	}
}
