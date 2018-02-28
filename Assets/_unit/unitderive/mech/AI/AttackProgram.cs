using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProgram : CommandProgram
{
    CharacterType targetType;
    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.Attack;
    }
    public override void Move()
    {
        mechCon.unitList.NearUnitSearch(mechCon.myUnit,mechCon.unitList.enemyList.FindAll(x=>x.charaType==targetType));
    }
}
