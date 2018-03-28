using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    BuildID bID;
    BuildManager buildManager;
    PlayerItemManager pItemManager;
    ItemTextManager itemTextManager;
    public UnityEngine.UI.Button button;
    private void Start()
    {
        //buttonを押した時に実行する関数を設定
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);

        //compornentをget
        pItemManager = CompornentUtility.FindCompornentOnScene<PlayerItemManager>() ;
        buildManager = CompornentUtility.FindCompornentOnScene<BuildManager>() ;
        itemTextManager = CompornentUtility.FindCompornentOnScene<ItemTextManager>();
        pItemManager.ItemQuantityChanged += ChangeButtonActive;
    }
    void ChangeButtonActive(int n)
    {
        button.interactable = (buildManager.CheckCanBuild(bID));
    }
    public void OnPointerEnter(PointerEventData e)
    {
        foreach (var i in buildManager.buildDataTable[(int)bID].consumptionItemData)
        {
            if (itemTextManager.GetItemTextHashData[i.Key] != null)
            {
                itemTextManager.GetItemTextHashData[i.Key].SetConsumptionText(i.Value);
            }
        }
    }
    public void OnPointerExit(PointerEventData e)
    {
        itemTextManager.AllTextConsumptionZero();
    }
    void OnClick()
    {
        buildManager.BuildNum=bID;
    }
}
