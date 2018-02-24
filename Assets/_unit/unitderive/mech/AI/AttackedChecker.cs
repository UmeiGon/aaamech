using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedChecker : EdgeChecker
{
    float preHelth;
   public override bool Check()
    {
        if (mech == null) return false;
        if (mech.Helth<preHelth)
        {
            preHelth = mech.Helth;
            return true;
        }
        preHelth = mech.Helth;
        return false;
       
    }
}
