using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {


	private delegate void StatePointer();
	private StatePointer currentState;
	private StatePointer OkState;

	public bool isEvil = true;
	public float Health = 3f;
	public float Scale = 1f;
	
	public float loResponseFreq;
    public float hiResponseFreq;

	// Original Color
	private Color OriginalColor;

	//------------------------- System functions.
	// Use this for initialization
	void Start () 
	{
		// Testing only///
		OriginalColor = gameObject.GetComponent<SpriteRenderer>().color;

		setStateOk();
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState();
	}

	// ------------------- Object updates...
	void setFrequency(float LowFrequency, float HighFrequency)
	{
		loResponseFreq = LowFrequency;
		hiResponseFreq = HighFrequency;
	}
	private void setSize(float size)
	{
	}
	//------------------------------ States

	private void IsHurting()
	{
		Health -= Time.deltaTime;
        Shake();
		if (Health < 0)
		{
			die();
			gameObject.SetActive(false);
			//Destroy(gameObject);
		}
	}

	private void isOKRegenerate()
	{
	}

    private void isOK()
	{
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
        Scale += 0.1f;
        setSize(Scale);
		Game.instance.SetDie(this.gameObject);
		//Destroy(gameObject);
	}

    void Shake()
    {
        Vector3 newPosition = gameObject.transform.position;
        newPosition.x += (Random.value - .5f) * .2f;
        newPosition.y += (Random.value - .5f) * .2f;
        if (Mathf.Sqrt(Mathf.Pow(newPosition.x, 2f) + Mathf.Pow(newPosition.y, 2f)) != Game.instance.radius)
            gameObject.transform.SetPositionAndRotation(newPosition, gameObject.transform.rotation);
    }
}