using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivedAttackEdgeChecker : EdgeChecker
{
    bool attackedFlag=false;
    void GotDamageCheck(Unit _unit)
    {
        attackedFlag = true;
    }
    protected override void Init()
    {
        MechCon.myUnit.AddReceivedDamageAction(GotDamageCheck);
    }
    public override bool Check()
    {
        if (attackedFlag)
        {
            attackedFlag = false;
            return true;
        }
        return false;   
    }
}
