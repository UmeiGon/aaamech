using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelthRateEdgeChecker : EdgeChecker {
    public int helthRate=1;
    public int orMore=0;
    public int Par
    {
        get
        {
            return helthRate;
        }
        set
        {
            helthRate = Mathf.Clamp(value,1,100);
        }
    }
    public override bool Check()
    {
        //以下
        if (orMore==0)
        {
            return ((MechCon.myUnit.Helth / MechCon.myUnit.maxHelth * 100) <= helthRate);
        }
        else
        {//以上
            return ((MechCon.myUnit.Helth / MechCon.myUnit.maxHelth * 100) >= helthRate);
        }
        
    }
}
