﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全てのユニットはUnitを継承する。
//ユニット自体に動きなどを書かない、あくまで識別のためのクラス
public class Unit : MonoBehaviour
{
    private float helth;
    private bool lifeZeroDeathFlag=true;
    public GameObject selectEffect;
    public float maxHelth;
    public enum Army
    {
        p1,p2,p3,p4,Neutral
    }
    public Army armyTag;
    public float Helth
    {
        set { if (value > maxHelth) maxHelth = value; helth = value; }
        get { return helth; }
    }
    public float attack = 1.0f;
    public virtual void Death() {
        GameObject.Find("Parent").GetComponentInChildren<UnitLists>().DeleteUnit(this);
        Destroy(gameObject);
    }
    protected void LifeZeroDeath()
    {
        if (lifeZeroDeathFlag&&helth <= 0)
        {
            Death();
        }
    }
    virtual protected void Init()
    {  
        helth = maxHelth;
        GameObject.Find("Parent").GetComponentInChildren<UnitLists>().AddUnit(this);
        StartCoroutine(UnitUpdate());
    }
    IEnumerator UnitUpdate()
    {
        while (true)
        {
            LifeZeroDeath();
            yield return null;
        }
    }
    private void Start()
    {
        Init();

    }
}
