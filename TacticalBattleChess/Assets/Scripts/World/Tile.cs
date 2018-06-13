﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private SelectManager sm;
    public Material unmarked;
    public Material marked;
    public Material visit;
    public Material cloas;
    public GameObject character;
    public Material prevMat;

	// Use this for initialization

	void Start () {
		GameObject field = GameObject.Find("World");
        prevMat = unmarked;
        sm = field.GetComponent<SelectManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        EventManager.SelectTile(gameObject);
    }

     void OnMouseEnter()
    {
        EventManager.HoverTile(gameObject);
    }

    //bainstorming
    public void Effect()
    {

    }

    public void visited()
    {
       GetComponent<MeshRenderer>().material = visit;
        prevMat = visit;
    }

    public void closed()
    {
        GetComponent<MeshRenderer>().material = cloas;
        prevMat = cloas;
    }

    public void mark()
    {
        GetComponent<MeshRenderer>().material = marked;
    }
    public void unmark()
    {
        GetComponent<MeshRenderer>().material = prevMat;
    }
    public void refresh()
    {
        if (GetComponent<PFelement>().walkable)
        {
            GetComponent<MeshRenderer>().material = unmarked;
            prevMat = unmarked;
        }
        else
        {
            GetComponent<MeshRenderer>().material = visit;
            prevMat = visit;
        }
    }
    public void reset()
    {
        GetComponent<MeshRenderer>().material = unmarked;
        prevMat = unmarked;
    }
}