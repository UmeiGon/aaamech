using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelthDamagedChecker : EdgeChecker {
    public int par=1;
    public int upper=0;
    public int Par
    {
        get
        {
            return par;
        }
        set
        {
            par = Mathf.Clamp(value,1,100);
        }
    }
    public override bool Check()
    {
        //以下
        if (upper==0)
        {
            return ((mech.myUnit.Helth / mech.myUnit.maxHelth * 100) <= par);
        }
        else
        {//以上
            return ((mech.myUnit.Helth / mech.myUnit.maxHelth * 100) >= par);
        }
        
    }
}
