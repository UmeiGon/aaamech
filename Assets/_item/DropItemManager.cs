using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    public List<DropItem> dropItemList = new List<DropItem>();

    public void GetDropItems(Vector3 _pos, float getDistance = 60.0f)
    {
        foreach (var i in dropItemList)
        {
            if (Vector3.Distance(i.transform.position, _pos) <= getDistance)
            {
                i.Picked();
            }
        }
        dropItemList.RemoveAll(c=>c.IsLife==false);
    }

}
