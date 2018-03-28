using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemTextManager : MonoBehaviour
{
    Dictionary<int,ItemText> itemTextHashData;
    public Dictionary<int ,ItemText> GetItemTextHashData { get { return itemTextHashData; } }
    private void Start()
    {
        //itemtextのlistを作成
        List<ItemText> iTextList=new List<ItemText>();
        CompornentUtility.GetTopParent.GetComponentsInChildren(iTextList);

        //アイテムの個数が更新された時にテキストも更新するようにする。
        var pItemManager = CompornentUtility.FindCompornentOnScene<PlayerItemManager>();
        foreach (var i in iTextList)
        {
            pItemManager.ItemQuantityChanged += i.TextReload;
        }
        
        //dictionary作成
        itemTextHashData=iTextList.ToDictionary(x=>(int)x.itemID);
    }
    public void AllTextConsumptionZero()
    {
        foreach (var i in itemTextHashData)
        {
            i.Value.SetConsumptionText(0);
        }
    }
}
