﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int team;
    public int maxhealth = 10;
    public int health = 10;
    public int movment;
    public bool alive = true;
    public List<Ability> abilitys = new List<Ability>();
    //Unity preset

    public Tile standingOn;
    public Material material;


    // Use this for initialization
    void Start()
    {
    }

    public void Init()
    {
        Start();
        AbInit();
    }

    protected abstract void AbInit();

    public void DealDamage(int dmg)
    {
        health -= dmg;
        DamageTaken(dmg);
        if (health <= 0)
        {
            health = 0;
            Kill();
        }
    }
    public void Heal(int heal)
    {
        health += heal;

        if (health > maxhealth)
        {
            heal = heal - health - maxhealth;
            health = maxhealth;
        }
        OnHeal(heal);
    }
    
    public virtual void Effect(int z,GameHelper.EffectType type)
    {
        switch (type)
        {
            case GameHelper.EffectType.Fire:
            case GameHelper.EffectType.Thunder:
            case GameHelper.EffectType.Light:
                DealDamage(z);
                break;
            case GameHelper.EffectType.Heal:
                HealChar(z);
                break;
            default:

                break;
        }
    }

    public void CastAbility(Ability ability,Tile tile)
    {
        ability.currentCd = ability.cd; //Todo do it in Ability Cast
        OnAbility(ability);
        ability.CastAbility(this, tile);
    }
    //buggy null exception
    public void Kill()
    {
        Field.Kill(this);
    }


    //EVENTS
    public delegate void DamageTakenAction(int dmg);
    public event DamageTakenAction OnDamageTaken;

    public void DamageTaken(int dmg)
    {
        if (OnDamageTaken != null)
        {
            OnDamageTaken(dmg);
        }
    }

    public delegate void HealAction(int dmg);
    public event HealAction OnHeal;

    public void HealChar(int heal)
    {
        if (OnDamageTaken != null)
        {
            OnHeal(heal);
        }
    }

    public delegate void AbilityAction(Ability ability);
    public event AbilityAction OnAbility;

    public void Ability(Ability ability)
    {
        if (OnAbility != null)
        {
            OnAbility(ability);
        }
    }


}
