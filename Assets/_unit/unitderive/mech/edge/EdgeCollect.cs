using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//指定したアイテムが指定した個数以上の時にtrueを返す。edge
public class EdgeCollect : CommandEdge {
    ItemID checkItemID;
    int needValue;
    PlayerItemManager pItemManager;
    private void Start()
    {
        var p=GameObject.Find("Parent");
        pItemManager = p.GetComponentInChildren<PlayerItemManager>();
    }
    public override bool Check()
    { 
       return pItemManager.itemDataTable[(int)checkItemID].Value>=needValue;
    }
}
