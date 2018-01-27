using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {


	private delegate void StatePointer();
	private StatePointer currentState;

    public bool isEvil = true;
	public float health = 5f;

	public float loResponseFreq;
    public float hiResponseFreq;
	public float maxHealth = 5f;
	public float maxSize = 5f;
	// Original Color
	private Color OriginalColor;




	//------------------------- System functions.
	// Use this for initialization
	void Start () 
	{

		// Testing only///
		OriginalColor = gameObject.GetComponent<SpriteRenderer>().color;



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

	// ------------------- Object updates...
	void setFrequency(float LowFrequency, float HighFrequency)
	{
		loResponseFreq = LowFrequency;
		hiResponseFreq = HighFrequency;
	}

	//------------------------------ States

	private void IsHurting()
	{
		health = health - Time.deltaTime;
		if (health < 0)
		{
			die();
			gameObject.active = false;
			//Destroy(gameObject);
		}
		else
		{
			setSize((1 - (health / maxHealth)) * maxSize + 1);
		}
	}
	private void setSize(float size)
	{
		gameObject.transform.localScale = new Vector3(size, size);
	}

    private void isOK()
	{
		//health = health + Time.deltaTime;
		health = (health < maxHealth) ? health + Time.deltaTime : maxHealth;
		setSize((1 - (health / maxHealth)) * maxSize + 1);  
	}
	public void dead()
	{

	}

	//--------------------------------- Interaction

	public void Activate(float newFrequency)
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

	//---------------------------------------- Changes of States.
    public void setStateDying()
	{
		currentState = IsHurting;
		print("I'm now hurting");

		// Testing only....
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        // Scale the cell big and small if it's in the response range
	}

    public void setStateOk()
	{
		currentState = isOK;
		print("I'm now OK");
		// Testing only....
		gameObject.GetComponent<SpriteRenderer>().color = OriginalColor;

		// transform back to 1:1 scale;
	}

	public void die()
	{
		currentState = dead;
		Game.instance.SetDie(this.gameObject);
		//Destroy(gameObject);
	}
}
