using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {


	private delegate void StatePointer();
	private StatePointer currentState;
	private StatePointer OkState;


	public bool isEvil = true;
	public float health = 5f;
	public float maxHealth = 5f;
	public bool canHeal = false;
	
	public float loResponseFreq;
    public float hiResponseFreq;

	public float maxSize = 5f;
	// Original Color
	private Color OriginalColor;




	//------------------------- System functions.
	// Use this for initialization
	void Start () 
	{

		// Testing only///
		OriginalColor = gameObject.GetComponent<SpriteRenderer>().color;

		if (canHeal)
			OkState = isOK;
		else
		{
			OkState = isOKRegenerate;
		}

		maxHealth = health;
		//currentState = isOK;
		//currentState = new StatePointer(isOK);
		setStateOk();

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState();
		MoveRandomly();
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

	void MoveRandomly()
	{
		Vector3 newPosition = gameObject.transform.position;
		newPosition.x = newPosition.x + ((Random.value - .5f) * .01f);
		newPosition.y = newPosition.y + ((Random.value - .5f) * .01f);
		if (Mathf.Sqrt(Mathf.Pow(newPosition.x, 2f) + Mathf.Pow(newPosition.y,2f)) != Game.instance.radius)
			gameObject.transform.SetPositionAndRotation(newPosition, gameObject.transform.rotation);
	}

	// ------------------- Object updates...
	void setFrequency(float LowFrequency, float HighFrequency)
	{
		loResponseFreq = LowFrequency;
		hiResponseFreq = HighFrequency;
	}
	private void setSize(float size)
	{
		gameObject.transform.localScale = new Vector3(size, size);
	}
	//------------------------------ States

	private void IsHurting()
	{
		health -= Time.deltaTime;
		if (health < 0)
		{
			die();
			gameObject.SetActive(false);
			//Destroy(gameObject);
		}
		else
		{
			setSize((1 - (health / maxHealth)) * maxSize + 1);
		}
	}

	private void isOKRegenerate()
	{
		health = (health < maxHealth) ? health + Time.deltaTime : maxHealth;
		setSize((1 - (health / maxHealth)) * maxSize + 1);
	}
	private void isOK()
	{
		//health = (health < maxHealth) ? health + Time.deltaTime : maxHealth;
		//setSize((1 - (health / maxHealth)) * maxSize + 1);  
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
		//print("I'm now hurting");

		// Testing only....
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        // Scale the cell big and small if it's in the response range
	}

    public void setStateOk()
	{
		currentState = OkState;
		//print("I'm now OK");
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