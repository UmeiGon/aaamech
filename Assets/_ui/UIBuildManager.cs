using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//素材が足りてるかによってボタンを出したり出さなかったりする。
public class UIBuildManager : MonoBehaviour
{
    List<BuildButton> buildButtons=new List<BuildButton>();
    BuildManager buildManager;
    private void Start()
    {
        var p = GameObject.Find("Parent");
        PlayerItemManager.GetInstance().changeTrigger += BuildButtonReload;
        buildManager = p.GetComponentInChildren<BuildManager>();
        p.GetComponentsInChildren(buildButtons);
        BuildButtonReload();
    }

    void BuildButtonReload()
    {
        foreach (var i in buildButtons)
        {
            if (buildManager.CanBuildCheck(i.BID))
            {
                i.button.interactable = true;
            }
            else
            {
                i.button.interactable = false;
            }
        }
    }
}

