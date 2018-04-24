using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBaseNodeActivity : NodeActivity {
    public override void ChangeTrigger()
    {
        mechCon.SetMode(MechController.Mode.Idle);
    }
    public override void MoveUpdate()
    {
        if (mechCon.targetUnit == null)
        {
            var unitC = CompornentUtility.FindCompornentOnScene<UnitListCabinet>();
            mechCon.targetUnit = unitC.SearchNearUnit(mechCon.myUnit, unitC.BaseList);
            if (mechCon.targetUnit)
            {
                mechCon.SetMode(MechController.Mode.Chase);
            }
            if (mechCon.targetUnit == null && mechCon.MechMode != MechController.Mode.Idle)
            {
                mechCon.SetMode(MechController.Mode.Idle);
            }
        }
        else if (mechCon.MechMode==MechController.Mode.Chase&&mechCon.TargetRangeInCheck(3.0f))
        {
            mechCon.SetMode(MechController.Mode.Idle);
        }
    }
}
