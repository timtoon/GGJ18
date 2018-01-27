using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public static Game instance;

    public int BadCells;
    public int GoodCells;

    public int totalCells = 100;
    public float badToGoodCellRatio = 2f; // 2:1 Bad cells for every one good cell

    // These are the response ranges
    public int loFrequency = 100;
    public int hiFrequency = 4000;

    // Delay the killing action of the frequency
    private static float attackDelay = 1f;
    private float attack = 1f;
    private int frequencyOld = 0;
    private int frequency;

    // This should get accessible only by a method, not public
    public bool OKToKillCells;

    // Parameters for Cell frequency response range
    public int startingRange = 2000;
    public int randomOffset = 950;
    public int randomVariance = 475;
    public int difficultyModifier = 475; // Overlap between Bad/Good Cells; lower is harder. 475 is the max, 1 the min.

    private Cell cellPrefab;

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
        // Init the alowed Frequency range
        startingRange = (loFrequency + hiFrequency) / 2;
        randomOffset = startingRange / 2;
        randomVariance = randomOffset / 2;

        // Populate with 100 or so Cell objects
        int badCells = (int)(totalCells * (badToGoodCellRatio / badToGoodCellRatio + 1));
        int goodCells = totalCells - badCells;

        for (int i = 0; i < badCells; i++)
        {
//            var cellInstance = Instantiate(CreateCell(true), new Vector2(0f,0f), new Quaternion(0,0,0,0));
        }

        for (int i = 0; i < goodCells; i++)
        {
//            var cellInstance = Instantiate(CreateCell(false), new Vector2(0f,0f), new Quaternion(0,0,0,0));
        }

        // randomize Cell positions
	}
	
	// Update is called once per frame
	void Update () 
	{
        OKToKillCells = OkToKillCells();

        //// This doesn't seem very efficient, counting 100 objects every frame.
        //// Would you rather update the count only when a Cell dies?
        //var badCellCount = GameObject.FindGameObjectsWithTag("BadCell").Length;
        //var goodCellCount = GameObject.FindGameObjectsWithTag("GoodCell").Length;

        //if (goodCellCount < 1)
        //{
        //    loseLevel();
        //}
        //else if (badCellCount < 1)
        //{
        //    winLevel();
        //}
    }

    public void SetDie(bool isEvil)
	{
		if (isEvil)
		{
			BadCells--;
			if (BadCells < 1)
			{
				winLevel();		
			}
		}
		else
		{
			GoodCells--;
			if (GoodCells < 1)
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

    // Map the screen width to the max hiFrequency, then add the lowest value
    // This shouldn't happen outside debugging, but I'm accounting for when the mouse goes outside the screen
    private int GetFrequency() {
        var f = (int) (Input.mousePosition.x * (hiFrequency - loFrequency) / Screen.width) + loFrequency;

        if (f < loFrequency)
        {
            return loFrequency;
        } 
        else if (f > hiFrequency)
        {
            return hiFrequency;
        }
        return f;
    }

    // Delay the StartKilling flag until Frequency is consistent for attackDelay seconds
    private bool OkToKillCells() {
        frequency = GetFrequency();

        if (frequency == frequencyOld)
        {
            if (attack <= 0)
            {
                return true;
            }
            else
            {
                attack -= Time.deltaTime;
            }
        }
        else
        {
            attack = attackDelay;
            frequencyOld = frequency;
        }
        return false;
    }

    private Cell CreateCell(bool isEnemy = false)
    {
        var c = new Cell();

        if(isEnemy) {
            c.tag = "BadCell";
        } else {
            c.tag = "GoodCell";
        }

        c.isEvil = isEnemy;

        var range = startingRange - ((Random.value * randomOffset) + randomOffset/2);

        if(isEnemy) {
            range += difficultyModifier / 2;
        } else {
            range -= difficultyModifier / 2;
        }

        c.loResponseFreq = range - Random.value * randomVariance;
        c.hiResponseFreq = range + Random.value * randomVariance;

        return c;
    }
}
