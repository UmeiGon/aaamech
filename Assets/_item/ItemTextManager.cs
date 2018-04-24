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
        CompornentUtility.TopParent.GetComponentsInChildren(iTextList);
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
