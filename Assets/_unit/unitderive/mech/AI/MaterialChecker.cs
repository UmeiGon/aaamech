using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChecker :EdgeChecker {
    public int needValue=0;
    public int itemNum;
    PlayerItemManager pItemMane;
    bool init=false;
    public override bool Check()
    {
        if (!init)
        {
            pItemMane=GameObject.Find("Parent").GetComponentInChildren<PlayerItemManager>();
            init = true;
        }
        return (pItemMane.itemDataTable[itemNum].Value >= needValue) ;
    }
}
