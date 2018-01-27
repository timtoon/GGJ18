using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public bool isEvil = true;
    public float loResponseFreq;
    public float hiResponseFreq;
    public int health = 5;

    private Vector2 position;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(health <= 0) {
            // Play death animation
            // Destroy(this,1);
        }

        var frequency = 100; // Get this from the current input

        if(frequency >= loResponseFreq && frequency <= hiResponseFreq) {
            StartVibrating();
            health--;
        } else {
            StopVibrating();
        }
	}

    void StartVibrating() {
        // Scale the cell big and small if it's in the response range
    }

    void StopVibrating() {
        // transform back to 1:1 scale;
    }
}
