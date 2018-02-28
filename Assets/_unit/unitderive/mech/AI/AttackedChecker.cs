using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedChecker : EdgeChecker
{
    bool attackedFlag=false;
    void GotDamageCheck(Unit _unit)
    {
        attackedFlag = true;
        mech.targetUnit = _unit;
    }
    bool init = false;
   public override bool Check()
    {
        if (!init)
        {
            mech.myUnit.gotDamage += GotDamageCheck;
            init = true;
        }
        if (attackedFlag)
        {
            attackedFlag = false;
            return true;
        }
        return false;   
    }
}
