﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayScript : MonoBehaviour {



    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        Game gobj = FindObjectOfType(typeof(Game)) as Game;

        float health = ((float)gobj.GoodCells) / ((float) gobj.GoodCells + gobj.BadCells);
        int health_disp = (int) Mathf.Round(health * 100f);
        GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "Health\n" + health_disp.ToString() + "%";;
    }
}
