using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//全てのユニットはUnitを継承する。
//ユニット自体に動きなどを書かない、あくまで識別のためのクラス
public abstract class  Unit : MonoBehaviour
{
    private float helth;
    private Action deathActions;
    private Action<Unit> ReceivedDamageAction;
    public GameObject selectEffect;
    public float maxHelth;
    public ItemID itemid = 0;
    public int itemValue = 0;
    public float radius = 5.0f;
    public enum Army
    {
        p1, p2, p3, p4, Neutral
    }
    public Army armyTag;
    public float Helth
    {
        set
        {
            if (value > maxHelth)
            {
                helth = maxHelth;
            }
            else
            {
                helth = value;
            }
        }
        get { return helth; }
    }
    public void AddReceivedDamageAction(Action<Unit> received_acition)
    {
        ReceivedDamageAction += received_acition;
    }
    public void AddDeathAction(Action _action)
    {
        deathActions += _action;
    }
    //unitからunitにダメージを与える時の関数
    public void SetDamage(float _damage, Unit _unit)
    {
        Helth -= _damage;
        if (ReceivedDamageAction != null) ReceivedDamageAction(_unit);
    }
    public float attack = 1.0f;
    void DiePraparation()
    {
        CompornentUtility.FindCompornentOnScene<UnitListCabinet>().DeleteUnit(this);
        if (itemValue > 0)
        {
            CompornentUtility.FindCompornentOnScene<ItemManager>().itemDataTable[(int)itemid].Value += itemValue;
        }
    }
    protected void LifeZeroDeath()
    {
        if (helth <= 0)
        {
            deathActions();
            Destroy(gameObject);
        }
    }
    virtual protected void Init() { }
    private void BaseInit()
    {
        helth = maxHelth;
        AddDeathAction(DiePraparation);
        CompornentUtility.FindCompornentOnScene<UnitListCabinet>().AddUnit(this);
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }

    protected void Start()
    {
        BaseInit();
        Init();
    }
}
