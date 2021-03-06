﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : SimplePlayer {


    private Game game;
    private World world;
    public static HumanController hc;
    public int ap = 2;
    public int maxap = 2;
    public int freeMove = 1;
    // Use this for initialization
    void Start()
    {
        GameObject g = GameObject.Find("World");
        game = g.GetComponent<Game>();
        world = g.GetComponent<World>();
        GetHumanController();
    }

    public  static HumanController GetHumanController()
    {
        if (hc == null)
        {
            hc = new HumanController();
        }
        return hc;
    }


    Character selectedCharacter;
    Ability selectedAbility;
    Tile hoveredTile;
    
    List<Tile> path = new List<Tile>();
    List<Tile> marked = new List<Tile>();
    List<Tile> markAb = new List<Tile>();
    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }
    public bool SelectCharacter(Character character)
    {
        if (world.SelectCharacter(character, false))
        {
            selectedCharacter = character;
            character.standingOn.tilehelper.Select();
            return true;
        }
        return false;
    }

    public bool CastAbility(Tile tile)
    {
        if (markAb.Contains(tile) && world.CastAbility(selectedCharacter, selectedAbility, tile))
        {
            return true;
        }
        return false;
    }

    public bool SelectAbility(Ability ability, Character character)
    {
        selectedAbility = ability;
        MarkAbility(ability, character.standingOn);
        return true;
    }
    public bool Move(Tile tile)
    {
        if (marked.Contains(tile) && world.Move(tile, selectedCharacter))
        {
            return true;
        }
        return false;
    }
    public void Hover(Tile tile)
    {
        if (hoveredTile != null)
        {
            hoveredTile.tilehelper.Undo();
            if (hoveredTile.GetCharacter() != null && game.GetComponent<UiHandler>().markActive)
            {
                game.GetComponent<UiHandler>().HideMarker();
            }
            World.indicator.Refresh();
        }
        if (tile.GetCharacter() != null)
        {
            game.GetComponent<UiHandler>().Mark(tile.GetCharacter());
        }
        tile.tilehelper.Hover();
        hoveredTile = tile;
        World.indicator.Refresh();
    }
    public void DrawPath(Tile tile)
    {
        MarkPath(tile);
    }

    Tile abilityDrawn = null;

    public void AbilityHover(Tile tile)
    {

        if (abilityDrawn != null)
        {
            World.indicator.RemoveNoCast();
            abilityDrawn = null;
        }
        if (markAb.Contains(tile))
        {
            if (hoveredTile != null)
            {
                if (!markAb.Contains(hoveredTile))
                {
                    hoveredTile.tilehelper.Undo();
                    if (hoveredTile.GetCharacter() != null && game.GetComponent<UiHandler>().markActive)
                    {
                        game.GetComponent<UiHandler>().HideMarker();
                    }
                }
                hoveredTile = null;
            }
            World.indicator.DrawNotCastedAbility(selectedAbility, tile);
            abilityDrawn = tile;
        }
        else
        {
            Hover(tile);
        }
    }

    public void MarkRange()
    {

        for (int i = 0; i < marked.Count; i++)
        {
            if (marked[i] != null && marked[i] != selectedCharacter.standingOn)
            {
                marked[i].tilehelper.Range();
            }
        }
        World.indicator.Refresh();
    }
    void MarkAbility(Ability ability, Tile tile)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.standingOn.tilehelper.Select();
        }
        markAb = ability.PossibleCasts(tile.GetCharacter(), tile);
        for (int i = 0; i < markAb.Count; i++)
        {
            if (markAb[i] != null && markAb[i] != tile.GetCharacter().standingOn)
            {
                markAb[i].tilehelper.Ability();
            }
        }
    }

    void MarkPath(Tile target)
    {
        
        for (int i = 0; i < path.Count; i++)
        {
            path[i].tilehelper.Range();

        }
        path = world.GetPath(target, selectedCharacter.movment);
        World.indicator.Refresh();
        //path.Add(target);
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].Walkable())
            {
                path[i].tilehelper.Path();
            }
        }
    }

    public void UnSelect()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.standingOn.tilehelper.Standard();
        }
        World.indicator.Refresh();
        for (int i = 0; i < marked.Count; i++)
        {
            if (marked[i] != null)
            {
                marked[i].tilehelper.Standard();
            }
        }
        marked = new List<Tile>();
        path = new List<Tile>();
    }

    public void UnAbility()
    {
        if (abilityDrawn != null)
        {
            World.indicator.RemoveNoCast();
            abilityDrawn = null;
        }
        selectedCharacter.standingOn.tilehelper.Standard();
        for (int i = 0; i < markAb.Count; i++)
        {
            if (markAb[i] != null && markAb[i] != selectedCharacter.standingOn)
            {
                markAb[i].tilehelper.Standard();
            }
        }
        markAb = new List<Tile>();
    }
 
    //Finish Methods
    public override void FinishSelecting(List<Tile> inRange)
    {
        marked = inRange;
        hc.FinischSelecting();
    }
    public override void FinishedMoving()
    {
        hc.FinishMoving();
        MoveAction();
    }

    public override void FinishedAbility()
    {
        if (abilityDrawn != null)
        {
            World.indicator.RemoveNoCast();
            abilityDrawn = null;
        }
        hc.FinishedAbility();
        ActionAbility();
    }


    //Player
    public override void TurnStart()
    {
        GetHumanController(); //Bug fix 
        hc.Next(this);
        hc.Turn();
        ap = maxap;
        freeMove = 1;
    }

    public void MoveAction()
    {
        if (freeMove > 0)
        {
            freeMove--;
        }
        else
        {
            if (--ap <= 0)
            {
                Finish();
            }
        }
    }

    public void ActionAbility()
    {
        if (--ap <= 0)
        {
            Finish();
        }
    }

    public override void Finish()
    {
        hc.Finish();
        game.FinishTurn();
    }

    public override void KillCharacter(Character character)
    {
        units.Remove(character);
    }
}
