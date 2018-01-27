using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public static Game instance;
	public int GoodCells;
	public int BadCells;

    private List<Cell> Cells;
    private int loFrequency;
    private int hiFrequency;
    private int difficulty = 100; // Set the difference of the frequency response range offset between good/evil Cells
    private int responseVariance = 100;

    private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;
		DontDestroyOnLoad(instance);
	}

    // Use this for initialization
	void Start () 
	{
        // Init the playfield
        // Populate with 100 or so Cells
        Cells.Add(CreateGoodCell());

        // These are the response ranges
        loFrequency = 100;
        hiFrequency = 4000;

        // randomize their position
        // create the Cells (51%+ evil)
	}
	
	// Update is called once per frame
	void Update () 
	{

        // Get the X position of the mouse
        var frequency = Input.mousePosition.x;


        // Get count of cells where gameObject.tag == 'Enemy'
        // if Cells.isEvil = true == 0?
        // You win!

        // if Cells.isEvil = false == 0?
        // You lose!
	}

    public void SetDie(bool isEvil)
	{
		if (isEvil)
		{
			BadCells--;
			if (BadCells <= 0)
			{
				winLevel();		
			}
		}
		else
		{
			GoodCells--;
			if (GoodCells <= 0)
			{
				loseLevel();
			}
		}
	}
	public void winLevel()
	{
		print("You won the level");
	}
	public void loseLevel()
	{
		print("You lost the level");
	}

    private Cell CreateGoodCell()
    {
        //instantiate Cell
        var c = new Cell();
        c.tag = "Player";
        c.isEvil = false;

        // set the response range
        c.loResponseFreq = loFrequency - responseVariance - (Random.value * responseVariance);
        c.hiResponseFreq = hiFrequency - responseVariance + (Random.value * responseVariance);

        return c;

    }

    private Cell CreateBadCell()
    {
        //instantiate Cell
        var c = new Cell();
        c.tag = "Enemy";

        // set the response range
        c.loResponseFreq = loFrequency + responseVariance - (Random.value * responseVariance);
        c.hiResponseFreq = hiFrequency + responseVariance + (Random.value * responseVariance);

        return c;

    }
}
