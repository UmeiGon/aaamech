using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerItemManager:MonoBehaviour
{

    public Dictionary<int, ItemData> itemDataTable;
    public Action<int> ItemQuantityChanged = null;
    PlayerItemManager()
    {
        itemDataTable = new Dictionary<int, ItemData>();
        foreach (var a in Enum.GetValues(typeof(ItemID)))
        {
            itemDataTable.Add((int)a,new ItemData());
        }
        //item数が更新された時にchangeTriggerが実行されるように
        foreach (var i in itemDataTable)
        {
            i.Value.valueChanged += ItemChangeTriggerF;
        }
        foreach (var i in itemDataTable)
        {
            i.Value.valueChanged(i.Value.Value);
        }
    }

    //貰ったitemlistを
    public void UsingItem(Dictionary<int, int> using_items)
    {
        foreach (var i in using_items)
        {
            itemDataTable[i.Key].Value -= i.Value;
        }
    }
    //なんらかのアイテムの数が変更されるたび呼ばれる。
    void ItemChangeTriggerF(int i)
    {
        if (ItemQuantityChanged != null) ItemQuantityChanged(i);
    }
}
