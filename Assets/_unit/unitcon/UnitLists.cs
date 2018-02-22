using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//あらゆるunitは生成されたときにunitlistにインスタンスを渡し、死ぬときにunitlistのインスタンスを消す
public class UnitLists : MonoBehaviour
{
    public List<MechUnit> playerMechList = new List<MechUnit>();
    public List<CharacterUnit> playerList = new List<CharacterUnit>();
    public List<CharacterUnit> charaList = new List<CharacterUnit>();
    public List<CharacterUnit> enemyList = new List<CharacterUnit>();
    public List<MaterialUnit> matList = new List<MaterialUnit>();
    public List<BuildUnit> buildList = new List<BuildUnit>();
   
    // Use this for initialization
    void Start()
    {
        var p = GameObject.Find("Parent");
    }
    public void AddUnit(Unit _unit)
    {
        if (_unit is CharacterUnit&&_unit.armyTag == Unit.Army.p1)
        {
            playerList.Add((CharacterUnit)_unit);
            if (_unit is MechUnit)
            {
                playerMechList.Add((MechUnit)_unit);
            }
        }  
        if (_unit is CharacterUnit)
        {
            charaList.Add((CharacterUnit)_unit);
            if (_unit.armyTag!=Unit.Army.p1)
            {
                enemyList.Add((CharacterUnit)_unit);
            }
        }
        if (_unit is MaterialUnit)
        {
            matList.Add((MaterialUnit)_unit);
        }
        if (_unit is BuildUnit)
        {
            buildList.Add((BuildUnit)_unit);
        }
    }
    public void DeleteUnit(Unit _unit)
    {
        if (_unit is CharacterUnit && _unit.armyTag == Unit.Army.p1)
        {
            playerList.Remove((CharacterUnit)_unit);
            if (_unit is MechUnit)
            {
                playerMechList.Remove((MechUnit)_unit);
            }
        }
        if (_unit is CharacterUnit)
        {
            charaList.Remove((CharacterUnit)_unit);
            if (_unit.armyTag != Unit.Army.p1)
            {
                enemyList.Remove((CharacterUnit)_unit);
            }
        }
        if (_unit is MaterialUnit)
        {
            matList.Remove((MaterialUnit)_unit);
        }
        if (_unit is BuildUnit)
        {
            buildList.Remove((BuildUnit)_unit);
        }
    }
    public Unit NearUnitSearch<T>(Unit main_unit, List<T> unit_list)
        where T : Unit
    {
        Unit e = null ;
        foreach (var i in unit_list)
        {
            if (e == null)e=i;
            if (Vector3.Distance(main_unit.transform.position, i.transform.position) < Vector3.Distance(main_unit.transform.position, e.transform.position))
            {
                e = i;
            }
        }
        return e;
    }
    void Reload<T>(List<T> a)
        where T : Unit
    {
        a.RemoveAll(s => s == null);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
