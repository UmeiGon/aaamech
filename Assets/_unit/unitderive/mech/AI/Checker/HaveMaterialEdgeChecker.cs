using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveMaterialEdgeChecker :EdgeChecker {
    public int needValue=0;
    public int itemNum;
    ItemManager pItemMane;
    protected override void Init()
    {
        pItemMane = CompornentUtility.FindCompornentOnScene<ItemManager>();
    }
    public override bool Check()
    {
        return (pItemMane.itemDataTable[itemNum].Value >= needValue) ;
    }
}
