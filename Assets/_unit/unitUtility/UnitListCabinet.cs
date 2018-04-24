using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//あらゆるunitは生成されたときにunitlistにインスタンスを渡し、死ぬときにunitlistのインスタンスを消す
public class UnitListCabinet : MonoBehaviour
{
    List<MechUnit> playerMechList = new List<MechUnit>();
    List<CharacterUnit> playerList = new List<CharacterUnit>();
    List<CharacterUnit> charaList = new List<CharacterUnit>();
    List<CharacterUnit> enemyList = new List<CharacterUnit>();
    List<MaterialUnit> matList = new List<MaterialUnit>();
    List<BuilderUnit> buildList = new List<BuilderUnit>();
    List<BaseUnit> baseList = new List<BaseUnit>();

    public List<MechUnit> PlayerMechList { get { return playerMechList; } }
    public List<CharacterUnit> PlayerList { get { return playerList; } }
    public List<CharacterUnit> CharaList { get { return charaList; } }
    public List<CharacterUnit> EnemyList { get { return enemyList; } }
    public List<MaterialUnit> MatList { get { return matList; } }
    public List<BuilderUnit> BuildList { get { return buildList; } }
    public List<BaseUnit> BaseList { get { return baseList; } }
 
    public void AddUnit(Unit _unit)
    {
        if (_unit is CharacterUnit && _unit.armyTag == Unit.Army.p1)
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
            if (_unit.armyTag != Unit.Army.p1)
            {
                enemyList.Add((CharacterUnit)_unit);
            }
        }
        if (_unit is MaterialUnit)
        {
            matList.Add((MaterialUnit)_unit);
        }
        if (_unit is BuilderUnit)
        {
            buildList.Add((BuilderUnit)_unit);
        }
        if (_unit is BaseUnit)
        {
            baseList.Add((BaseUnit)_unit);
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
        if (_unit is BuilderUnit)
        {
            buildList.Remove((BuilderUnit)_unit);
        }
        if (_unit is BaseUnit)
        {
            baseList.Remove((BaseUnit)_unit);
        }
    }
    public Unit SearchNearUnit<T>(Unit main_unit, List<T> unit_list)
        where T : Unit
    {
        Unit u = null;
        foreach (var i in unit_list)
        {
            if (u == null || Vector3.Distance(main_unit.transform.position, i.transform.position) < Vector3.Distance(main_unit.transform.position, u.transform.position))
            {
                u = i;
            }
        }
        return u;
    }
    void Reload<T>(List<T> a)
        where T : Unit
    {
        a.RemoveAll(s => s == null);
    }
}
