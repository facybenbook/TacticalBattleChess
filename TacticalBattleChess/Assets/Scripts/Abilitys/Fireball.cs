﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability {


    public GameObject prefab;
    public float speed = 0.2f;
    public int damage = 10;




    int directionOffset;
    Tile from;
    public override void CastAbility(Character character, Tile target)
    {
     directionOffset =   from.neighboors.IndexOf(target);
        if (directionOffset == -1)
        {
            return;
        }
        from = from.neighboors[directionOffset];
        GameObject g = Instantiate(prefab);
        StartCoroutine(Animation(g));
   
         
    }

    public override List<Tile> possibleCasts(Character character, Tile from)
    {
        this.from = from;
        return from.neighboors;
    }
 

    IEnumerator Animation(GameObject g)
    {
        while (from != null && from.Walkable() != false)
        {
            g.transform.position = new Vector3(from.transform.position.x, from.transform.position.y, -4);
            from = from.neighboors[directionOffset];
            yield return new WaitForSeconds(speed);
        }
        if (from.GetComponent<Tile>().GetCharacter() != null)
        {
            from.GetComponent<Tile>().GetCharacter().DealDamage(damage);
        }
        Destroy(g);
    }

    // Use this for initialization
    void Start () {
    }
	
}
