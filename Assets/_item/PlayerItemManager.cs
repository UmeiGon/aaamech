using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager
{

    public Dictionary<int, ItemData> itemDataTable;
    public delegate void ItemChangeTrigger();
    public ItemChangeTrigger changeTrigger = null;
    static PlayerItemManager instance = null;
    public static PlayerItemManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerItemManager();
        }
        return instance;
    }
    PlayerItemManager()
    {
        itemDataTable = new Dictionary<int, ItemData>
        {
            { (int)ItemID.Stone, new ItemData() },
            { (int)ItemID.Wood, new ItemData() }
        };
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
        if (changeTrigger != null) changeTrigger();
    }
}
