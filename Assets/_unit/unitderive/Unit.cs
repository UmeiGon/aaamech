using System.Collections;
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
    public delegate void GotDamage(Unit _unit);
    public GotDamage gotDamage;
    public ItemID itemid = 0;
    public int itemValue = 0;
    public float radius = 0;
    public enum Army
    {
        p1,p2,p3,p4,Neutral
    }
    public Army armyTag;
    public float Helth
    {
        set { if (value > maxHelth) helth = maxHelth;
            else
            {
                helth = value;
            }
        }
        get { return helth; }
    }
    //unitからunitにダメージを与える時の関数
    public void SetDamage(float damage,Unit _unit)
    {
        Helth -= damage;
       if(gotDamage != null)gotDamage(_unit);
    }
    public float attack = 1.0f;
    public virtual void Death() {
        var p = GameObject.Find("Parent");
        p.GetComponentInChildren<UnitLists>().DeleteUnit(this);
        if (itemValue > 0)
        {
            p.GetComponentInChildren<PlayerItemManager>().itemDataTable[(int)itemid].Value += itemValue;
        }
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
