using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIItemManager : MonoBehaviour
{
    PlayerItemManager pItemMane;
    
    private void Awake()
    {
        pItemMane= GameObject.Find("Parent").GetComponentInChildren<PlayerItemManager>();
        //アイテム個数が変更された時にデリゲートでtextの関数を呼んで、textも更新。
        var uis=GetComponentsInChildren<UIItemText>();
        foreach (var i in uis)
        {        
            pItemMane.itemDataTable[(int)i.ID].valueChanged += i.TextReload;
            pItemMane.itemDataTable[(int)i.ID].Value=pItemMane.itemDataTable[(int)i.ID].Value;
        }
    }
}
