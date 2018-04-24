using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNodeActivity : NodeActivity
{
    public int targetType = 0;
    public override void ChangeTrigger()
    {
        mechCon.SetMode(MechController.Mode.Idle);
    }
    public override void MoveUpdate()
    {
        if (mechCon.targetUnit == null)
        {
            if (System.Enum.IsDefined(typeof(CharacterType), targetType))
            {
                mechCon.targetUnit = mechCon.unitList.SearchNearUnit(mechCon.myUnit, mechCon.unitList.EnemyList.FindAll(x => x.charaType == (CharacterType)targetType));    
            }
            else
            {
                mechCon.targetUnit = mechCon.unitList.SearchNearUnit(mechCon.myUnit, mechCon.unitList.EnemyList);
            }
            if (mechCon.targetUnit)
            {
                mechCon.SetMode(MechController.Mode.Chase);
            }
            if (mechCon.targetUnit == null && mechCon.MechMode != MechController.Mode.Idle)
            {
                mechCon.SetMode(MechController.Mode.Idle);
            }
        }
        else if (mechCon.MechMode == MechController.Mode.Chase && mechCon.TargetAttackRangeInCheck())
        {
            mechCon.SetMode(MechController.Mode.Attack);
        }

    }
}
