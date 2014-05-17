using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class depthSort : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LookAtCamera[] spriteRenderers = GameObject.FindObjectsOfType<LookAtCamera>();
        IEnumerable<LookAtCamera> sorted=spriteRenderers.OrderByDescending(s => (s.transform.position - transform.position).magnitude);
        for (int i = 0; i < sorted.Count(); i++)
        {
            sorted.ElementAt(i).depth = i;
        }

	}
}
