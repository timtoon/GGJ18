using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {


	private delegate void StatePointer();
	private static StatePointer currentState;

    public bool isEvil = true;
	public float health = 5f;

	private float loResponseFreq;
    private float hiResponseFreq;
	private float maxHealth;

    private Vector2 position;


	// Use this for initialization
	void Start () 
	{
		maxHealth = health;
		//currentState = isOK;
		//currentState = new StatePointer(isOK);
		setStateOk();
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState();
		/*
        if(health <= 0) {
            // Play death animation
            // Die, Destroy(this,1);
        }
		
        var frequency = 100; // Get this from the current input

        if(frequency >= loResponseFreq && frequency <= hiResponseFreq) {
            setStateDying();
            IsHurting();
        } else {
            setStateOk();
        }
		*/
	}

    void IsHurting()
	{
		health = health - Time.deltaTime;
		if (health < 0)
			die();
	}

    void isOK()
	{
		health = health + Time.deltaTime;
	}

    void Activate(float newFrequency)
	{
		if (newFrequency >= loResponseFreq && newFrequency <= hiResponseFreq)
		{
			setStateDying();
		}
		else
		{
			setStateOk();
		}
	}

    void setStateDying()
	{
		currentState = IsHurting;
        // Scale the cell big and small if it's in the response range
	}

    void setStateOk()
	{
		currentState = isOK;
        // transform back to 1:1 scale;
	}

    void die()
	{
		Game.instance.SetDie(isEvil);
		Destroy(gameObject);
	}
}
