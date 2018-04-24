using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemNodeActivity : NodeActivity
{
   public  int pickItemID=0;
    public override void ChangeTrigger()
    {
        mechCon.SetMode(MechController.Mode.Idle);
    }
    public override void MoveUpdate()
    {
        if (mechCon.targetUnit == null)
        {
            mechCon.targetUnit=mechCon.unitList.SearchNearUnit(mechCon.myUnit, mechCon.unitList.MatList.FindAll(x => x.matTag == (MaterialID)pickItemID));
            if (mechCon.targetUnit)
            {
                mechCon.SetMode(MechController.Mode.Chase);
            }
            if (mechCon.targetUnit == null && mechCon.MechMode != MechController.Mode.Idle)
            {
                mechCon.SetMode(MechController.Mode.Idle);
            }
        }
        else if (mechCon.MechMode == MechController.Mode.Chase&&mechCon.TargetAttackRangeInCheck())
        {
            mechCon.SetMode(MechController.Mode.Attack);
        }
    }
}
