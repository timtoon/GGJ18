using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

	public bool isEvil = true;
	public float Health = 3f;
	public float Scale = 1f;

    public bool isHurtingFlag = false;
	
	public float loResponseFreq;
    public float hiResponseFreq;

	//------------------------- System functions.
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(isHurtingFlag) {
            Shake();
            Health -= Time.deltaTime;
            if (Health < 0)
            {
                die();
                Destroy(gameObject);
            }
        }
	}

	// ------------------- Object updates...
	void setFrequency(float LowFrequency, float HighFrequency)
	{
		loResponseFreq = LowFrequency;
		hiResponseFreq = HighFrequency;
	}

	//--------------------------------- Interaction
	public void Activate(float newFrequency)
	{
        isHurting(newFrequency >= loResponseFreq && newFrequency <= hiResponseFreq);
	}

	//---------------------------------------- Changes of States.
    public void isHurting(bool isHurting = false)
	{
        isHurtingFlag = isHurting;
        if (isHurting)
        {
            //play hurting animation
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetBool("ishurting", true);

            //play hurting SFX
            AudioSource[] hurtingSFX = GetComponents<AudioSource>();
            hurtingSFX[Random.Range(0, 3)].Play();
        } else
        {
            //play idle animation
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetBool("ishurting", false);
        }
    }

	public void die()
	{
        Scale += 0.1f;
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