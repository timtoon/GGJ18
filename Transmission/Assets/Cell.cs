using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    private bool isEvil;
    private float lowResponseFreq;
    private float hiResponseFreq;
    private int health = 5;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(health <= 0) {
            // Play death animation
            // Die, Destroy(this,1);
        }

        if(frequencty >= lowResponseFreq && frequency <= hiResponseFreq) {
            health--;
        }
	}
}
