using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDrHerzLoseSFX : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        AudioSource[] loseSFX = GetComponents<AudioSource>();
        loseSFX[Random.Range(0, 3)].Play();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
