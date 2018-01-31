using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;

public class Game : MonoBehaviour 
{
	public static Game instance;

	public int GoodCells;
	//public float GoodCellHealth;
	public GameObject GoodCellObject;

	public int BadCells;
	//public float BadCellHealth;
	public GameObject BadCellObject;

	public float radius;
    public int cellLayer;

    public static int Level = 1;

	// Delay the killing action of the frequency
	private static float frequencyAttackDelay_Static = 1f;
	private float frequencyAttackDelay = 1f;
	private int frequencyOld = 0;
	private int frequency;
	public bool frequencyAttackEnable;

	// Parameters for Cell Frequency Response range
	private int cfr_startingRange = 3500;
	private int cfr_randomOffset = 950;
	private int cfr_randomVariance = 475;
	private int cfr_difficultyModifier = 300; // Overlap between Bad/Good Cells; lower is harder.
	public int CFRLoFrequency = 100;
	public int CFRHiFrequency = 4000;

    //private float LastFrequency;

    private List<GameObject> Cells = new List<GameObject>();

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;
		
	}

    // Use this for initialization
	void Start () 
	{
        if(Level != 1)
        {
            AudioSource[] endSFX = GetComponents<AudioSource>();
            endSFX[Random.Range(0, 3)].Play();
        }
        InitFrequency();
        SetDifficulty(Level);
		PopulateDish();
	}

	void InitFrequency()
	{
		cfr_startingRange = (CFRLoFrequency + CFRHiFrequency) / 2;
		cfr_randomOffset = cfr_startingRange / 2;
		cfr_randomVariance = cfr_randomOffset / 2;
	}

	//----------------------------------------- Input Handlers

	// Update is called once per frame
	void Update () 
	{
		DirectEffect();
		//DelayEffect();
	}
	void DelayEffect()
	{
        if (OkToKillCells())
			InputDataUpdateFrequency(frequency);
	}

	void DirectEffect()
	{
		// Get the X position of the mouse
		int f = GetFrequency();
		if (f != frequencyOld)
		{
//			print("Frequency: "+frequency);
			InputDataUpdateFrequency(f);
			frequencyOld = f;
		}
	}

	// Delay the StartKilling flag until Frequency is consistent for attackDelay seconds
	private bool OkToKillCells()
	{
		frequency = GetFrequency();

		if (frequency == frequencyOld)
		{
			if (frequencyAttackDelay <= 0)
			{
				return true;
			}
			else
			{
				frequencyAttackDelay -= Time.deltaTime;
			}
		}
		else
		{
			frequencyAttackDelay = frequencyAttackDelay_Static;
			frequencyOld = frequency;
		}
		return false;
	}

	// Map the screen width to the max hiFrequency, then add the lowest value
	// This shouldn't happen outside debugging, but I'm accounting for when the mouse goes outside the screen
	private int GetFrequency()
	{
		var f = (int)(Input.mousePosition.x * (CFRHiFrequency - CFRLoFrequency) / Screen.width) + CFRLoFrequency;

		if (f < CFRLoFrequency)
		{
			return CFRLoFrequency;
		}
		else if (f > CFRHiFrequency)
		{
			return CFRHiFrequency;
		}
		return f;
	}

	public void InputDataUpdateFrequency(float frequency)
	{
        foreach (var cell in Cells)
        {
            var cellScript = cell.GetComponent<Cell>();
            cellScript.Activate(frequency);
        }

        // just kill these cells
        var activeCells = Cells.Select(x => x.GetComponent<Cell>().loResponseFreq >= frequency && x.GetComponent<Cell>().hiResponseFreq <= frequency);

	}

	//----------------------------------------- End of game Checking


	public void SetDie(GameObject DeadCell)
	{
		
		if (DeadCell.GetComponent<Cell>().isEvil)
		{
			print("Bad Died");
			BadCells--;
			if (BadCells <= 0)
			{
				winLevel();		
			}
		}
		else
		{
			print("Good Died");
			GoodCells--;
			if (GoodCells <= 0)
			{
				loseLevel();
			}
		}
		Cells.Remove(DeadCell);
		//Cells.FindIndex(DeadCell)
	}

    public void winLevel()
	{
        print("You won the level");

        AudioSource[] deathSFX = GetComponents<AudioSource>();
        //deathSFX[Random.Range(9, 12)].Play();
        deathSFX[Random.Range(3, 6)].Play();

        Level++;
        SceneManager.LoadScene("GameScene");
	}

    public void loseLevel()
	{
		print("You lost the level");

        AudioSource[] deathSFX = GetComponents<AudioSource>();
        //deathSFX[Random.Range(6, 9)].Play();
        deathSFX[Random.Range(3, 6)].Play();

        Level = 1;
        SceneManager.LoadScene("LoseScreen");
	}

	//----------------------------------------- POPULATIONS

	public void PopulateDish()
	{
        // doubled the Cell counts to put each cell on its own even/odd layer
		for (int x = 1; x < GoodCells*2; x+=2)
		{
            cellLayer = x;
			Cells.Add(CreateGoodCell());
			print("Add Good Cells");
		}
		for (int x = 0; x < BadCells*2; x+=2)
		{
            cellLayer = x;
			Cells.Add(CreateBadCell());
			print("Add Bad Cells");
		}
	}

	private GameObject CreateCell(GameObject Copy)
	{
		return null;
	}

	public void SetCellFrequencyRange(Cell c)
	{

		var range = cfr_startingRange - ((Random.value * cfr_randomOffset) + cfr_randomOffset / 2);

		if (c.isEvil)
		{
			range += cfr_difficultyModifier / 2;
		}
		else
		{
			range -= cfr_difficultyModifier / 2;
		}

		c.loResponseFreq = range - Random.value * cfr_randomVariance;
		c.hiResponseFreq = range + Random.value * cfr_randomVariance;
	}

	private GameObject CreateGoodCell()
    {
		GameObject myCell = Instantiate(GoodCellObject);
		//instantiate Cell
		myCell.tag = "GoodCell";
		Cell c = myCell.GetComponent<Cell>();
        c.isEvil = false;

        SetCellFrequencyRange(c);
		PlaceCell(myCell);

		return myCell;
    }

	private GameObject CreateBadCell()
    {
		GameObject myCell = Instantiate(BadCellObject);
        myCell.tag = "BadCell";
		Cell c = myCell.GetComponent<Cell>();
		//c.isEvil = true;

		SetCellFrequencyRange(c);
		PlaceCell(myCell);

		return myCell;
    }

	private void PlaceCell(GameObject c)
	{
		Vector3 position = new Vector3();

        float angle = 6.28f * Random.value;
        float dist = (Random.value - 0.5f) * 2f * radius;

        position.x = Mathf.Cos(angle) * dist-3.5f;//(float)xPosition - 3;
        position.y = Mathf.Sin(angle) * dist;
		c.transform.SetPositionAndRotation(position, c.transform.localRotation);

        c.GetComponent<SpriteRenderer>().sortingOrder = cellLayer;
	}

    void SetDifficulty(int level)
    {
		var totalCells = Mathf.RoundToInt(Mathf.Pow (2, level)) + 2;
        var badToGoodCellRatio = 0.9f + (level * 0.4f);

        BadCells = (int) (totalCells * badToGoodCellRatio / (badToGoodCellRatio + 1));
        GoodCells = totalCells - BadCells;

        //Debug.Log("badToGoodCellRatio: "+badToGoodCellRatio+" BadCells: "+BadCells+ "GoodCells: "+GoodCells);

        cfr_difficultyModifier -= level * 25;   // Tighten the difference in frequencies until they're almost the same.

        // ...but not completely the same
        if (cfr_difficultyModifier < 10)
        {
            cfr_difficultyModifier = 10;
        }
    }

	public int getLevel() {
		return Level;
	}
}
