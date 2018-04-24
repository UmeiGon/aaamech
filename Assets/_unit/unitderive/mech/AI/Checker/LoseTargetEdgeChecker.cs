using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseTargetEdgeChecker : EdgeChecker
{
    Unit preTargetUnit;
    bool targetAdded; 
    protected override void Init()
    {
        preTargetUnit = null;
        targetAdded = false;
    }
    public override bool Check()
    {
        if (!preTargetUnit && MechCon.targetUnit)
        {
            preTargetUnit = MechCon.targetUnit;
            targetAdded = true;
        }

        if (targetAdded)
        {
            if (preTargetUnit)
            {
                return(preTargetUnit != MechCon.targetUnit);
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
