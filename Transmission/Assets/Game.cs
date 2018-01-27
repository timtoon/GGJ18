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

	private List<GameObject> Cells = new List<GameObject>();
    private int loFrequency;
    private int hiFrequency;


    //private int difficulty = 100; // Set the difference of the frequency response range offset between good/evil Cells
    private int responseVariance = 100;
	private float LastFrequency;
	

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
        loFrequency = 100;
        hiFrequency = 4000;

        // randomize their position
        // create the Cells (51%+ evil)
	}


	//----------------------------------------- Input Handlers

	// Update is called once per frame
	void Update () 
	{

        // Get the X position of the mouse
        float frequency = Input.mousePosition.x;
		//frequency = 5;
		if (frequency != LastFrequency)
		{
			GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "" +  frequency;
			print("Updating Frequency");
			InputDataUpdateFrequency(frequency);
			LastFrequency = frequency;
		}
		


		// Get count of cells where gameObject.tag == 'Enemy'
		// if Cells.isEvil = true == 0?
		// You win!

		// if Cells.isEvil = false == 0?
		// You lose!
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

    private GameObject CreateGoodCell()
    {
		GameObject myCell = Instantiate(GoodCellObject);
		//instantiate Cell
		//var c = new Cell();
		myCell.tag = "Player";
		Cell c = myCell.GetComponent<Cell>();
        c.isEvil = false;




		
		// set the response range
		
        c.loResponseFreq = loFrequency - responseVariance - (Random.value * responseVariance);
        c.hiResponseFreq = hiFrequency - responseVariance + (Random.value * responseVariance);

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
        c.loResponseFreq = loFrequency + responseVariance - (Random.value * responseVariance);
        c.hiResponseFreq = hiFrequency + responseVariance + (Random.value * responseVariance);

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
