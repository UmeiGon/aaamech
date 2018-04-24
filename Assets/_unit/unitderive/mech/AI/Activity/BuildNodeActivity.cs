using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNodeActivity : NodeActivity
{

    public override void ChangeTrigger()
    {
        mechCon.SetMode(MechController.Mode.Idle);
    }
    public override void MoveUpdate()
    {
        if (mechCon.targetUnit == null)
        {
            mechCon.targetUnit = mechCon.unitList.SearchNearUnit(mechCon.myUnit, mechCon.unitList.BuildList);
            if (mechCon.targetUnit)
            {
                mechCon.SetMode(MechController.Mode.Chase);
            }
            if (mechCon.targetUnit == null&& mechCon.MechMode != MechController.Mode.Idle)
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
