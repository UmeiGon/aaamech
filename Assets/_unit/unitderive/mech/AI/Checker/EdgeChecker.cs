using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EdgeChecker
{  
    MechController mech=null;
    public MechController MechCon
    {
        get { return mech; }
        set { mech = value;Init(); }
    }
    protected virtual void Init() { }
    public abstract  bool Check();
}
