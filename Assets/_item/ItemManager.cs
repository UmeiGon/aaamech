using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ItemManager:MonoBehaviour
{

    public Dictionary<int, ItemData> itemDataTable;
    public Action<int> ItemQuantityChanged = null;
    ItemManager()
    {
        itemDataTable = new Dictionary<int, ItemData>();
        foreach (var a in Enum.GetValues(typeof(ItemID)))
        {
            itemDataTable.Add((int)a,new ItemData());
        }
        //item数が更新された時にchangeTriggerが実行されるように
        foreach (var i in itemDataTable)
        {
            i.Value.AddValueChangedTrigger(ItemChangeTriggerF);
        }
        foreach (var i in itemDataTable)
        {
            i.Value.Value= i.Value.Value;
        }
    }
    //Debug用
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            foreach (var i in itemDataTable)
            {
                i.Value.Value += 40;
            }
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
