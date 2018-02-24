using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChecker :EdgeChecker {
    public int needValue=0;
    public ItemID itemNum;
    public override bool Check()
    {
        return (PlayerItemManager.GetInstance().itemDataTable[(int)itemNum].Value >= needValue) ;
    }
}
