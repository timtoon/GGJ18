using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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





	// Delay the killing action of the frequency
	private static float frequencyAttackDelay_Static = 1f;
	private float frequencyAttackDelay = 1f;
	private int frequencyOld = 0;
	private int frequency;
	public bool frequencyAttackEnable;

	// Parameters for Cell Frequency Response range
	public int cfr_startingRange = 2000;
	public int cfr_randomOffset = 950;
	public int cfr_randomVariance = 475;
	public int cfr_difficultyModifier = 475; // Overlap between Bad/Good Cells; lower is harder. 475 is the max, 1 the min.
	public int cfr_loFrequency = 100;
	public int cfr_hiFrequency = 4000;

	//private float LastFrequency;

	private List<GameObject> Cells = new List<GameObject>();

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;
		GameObject.DontDestroyOnLoad(instance);
		
	}

    // Use this for initialization
	void Start () 
	{
		// Init the playfield
		// Populate with 100 or so Cells
		populateDish();
		//Cells.Add(CreateGoodCell());

		// These are the response ranges
		initFrequency();

        // randomize their position
        // create the Cells (51%+ evil)
	}

	void initFrequency()
	{
		cfr_startingRange = (cfr_loFrequency + cfr_hiFrequency) / 2;
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
		frequencyAttackEnable = OkToKillCells();
		if (frequencyAttackEnable)
			InputDataUpdateFrequency(frequency);
	}

	void DirectEffect()
	{ 
		// Get the X position of the mouse
		int frequency = GetFrequency();
		//frequency = 5;
		if (frequency != frequencyOld)
		{
			GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "" + frequency;
			print("Updating Frequency");
			InputDataUpdateFrequency(frequency);
			frequencyOld = frequency;
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
		var f = (int)(Input.mousePosition.x * (cfr_hiFrequency - cfr_loFrequency) / Screen.width) + cfr_loFrequency;

		if (f < cfr_loFrequency)
		{
			return cfr_loFrequency;
		}
		else if (f > cfr_hiFrequency)
		{
			return cfr_hiFrequency;
		}
		return f;
	}


	public void InputDataUpdateFrequency(float frequency)
	{
		//using System.Linq;
		//int max = Cells.Count;
		for (int x = 0; x < Cells.Count; x++)
		{
			GameObject C = Cells.ElementAt<GameObject>(x);
			Cell myCell = C.GetComponent<Cell>();
			myCell.Activate(frequency);
		}
			//Cell C = Cells[x].get<Cell>();
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
	}
	public void loseLevel()
	{
		print("You lost the level");
	}










	//----------------------------------------- POPULATIONS

	public void populateDish()
	{
		for (int x = 0; x < GoodCells; x++)
		{
			Cells.Add(CreateGoodCell());
			print("Add Good Cells");
		}
		for (int x = 0; x < BadCells; x++)
		{
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
		//var c = new Cell();
		myCell.tag = "Player";
		Cell c = myCell.GetComponent<Cell>();
        c.isEvil = false;





		// set the response range
		SetCellFrequencyRange(c);
		//c.loResponseFreq = loFrequency - responseVariance - (Random.value * responseVariance);
		//c.hiResponseFreq = hiFrequency - responseVariance + (Random.value * responseVariance);

		placeCell(myCell);
        return myCell;

    }


	private GameObject CreateBadCell()
    {
		GameObject myCell = Instantiate(BadCellObject);
		//var c = new Cell();
		Cell c = myCell.GetComponent<Cell>();
		//c.isEvil = true;
		//c.tag = "Enemy";

		// set the response range
		SetCellFrequencyRange(c);
		//c.loResponseFreq = loFrequency + responseVariance - (Random.value * responseVariance);
		//c.hiResponseFreq = hiFrequency + responseVariance + (Random.value * responseVariance);

		placeCell(myCell);

		return myCell;
    }



	private void placeCell(GameObject c)
	{
		float xPosition;
		float DistFromCenter;
		float asquared;
		Vector3 position = new Vector3();

		DistFromCenter = (Random.value - 0.5f) * 2f * radius;
		xPosition = (Random.value - 0.5f) * 2f * radius;
		position.x = (float)xPosition;
		position.y = (Mathf.Abs(DistFromCenter - xPosition) / (DistFromCenter - xPosition)) * Mathf.Pow(Mathf.Abs(Mathf.Pow(DistFromCenter, 2) - Mathf.Pow(xPosition, 2)), 0.5f);

		c.transform.SetPositionAndRotation(position, c.transform.localRotation);
	}
}
