using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemID { Stone, Wood }
public class PlayerItemManager : MonoBehaviour {
   
    public Dictionary<int, ItemData> itemDataTable;
    public delegate void ItemChangeTrigger();
    public ItemChangeTrigger changeTrigger=null;
	// Use this for initialization
	void Awake () {
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
    }
    private void Start()
    {
        foreach (var i in itemDataTable)
        {
            i.Value.valueChanged(i.Value.Value);
        }
    }
    //貰ったitemlistを
    public void UsingItem(Dictionary<int,int> using_items)
    {
        foreach (var i in using_items)
        {
            itemDataTable[i.Key].Value -= i.Value;
        }
    }
    //なんらかのアイテムの数が変更されるたび呼ばれる。
    void ItemChangeTriggerF(int i)
    {
        if(changeTrigger!=null)changeTrigger();
    }
	// Update is called once per frame
	void Update () {
       
	}
}
