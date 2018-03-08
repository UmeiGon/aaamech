using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProgram : CommandProgram
{
    public int targetType=0;
    public override void ChangeTrigger()
    {
        mechCon.mode = MechController.Mode.Attack;
    }
    public override void Move()
    {
        if (mechCon.targetUnit == null)
        {
            Debug.Log("a");
            if (System.Enum.IsDefined(typeof(CharacterType), targetType))
            {
                mechCon.targetUnit = mechCon.unitList.NearUnitSearch(mechCon.myUnit, mechCon.unitList.enemyList.FindAll(x => x.charaType == (CharacterType)targetType));
            }
            else
            {//定義済みでないならenemylistから探す
                mechCon.targetUnit = mechCon.unitList.NearUnitSearch(mechCon.myUnit, mechCon.unitList.enemyList);
            }
            
        }
 
    }
}
