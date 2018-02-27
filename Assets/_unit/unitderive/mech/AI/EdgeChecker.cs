using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EdgeChecker
{
    public string edgeName="default";
    public MechUnit mech=null;
    public abstract  bool Check();
}
