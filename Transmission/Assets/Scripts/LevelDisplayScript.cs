using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayScript : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        GameObject gobg = GameObject.Find("GameObject");
        Game gobj = FindObjectOfType(typeof(Game)) as Game;
		int level = gobj.getLevel();
		GameObject.Find("Level").GetComponent<UnityEngine.UI.Text>().text = "Level\n" + level.ToString();
    }
}
