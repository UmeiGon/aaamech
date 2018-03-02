using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//素材が足りてるかによってボタンを出したり出さなかったりする。
public class UIBuildManager : MonoBehaviour
{
    List<BuildButton> buildButtons = new List<BuildButton>();
    List<UIItemText> iTextLIst = new List<UIItemText>();
    BuildButton selectButton;
    [SerializeField]
    Button createButton;
    public BuildButton SelectBuildButton
    {
        set
        {
            selectButton = value;
            BuildButtonReload();
            foreach (var ss in iTextLIst)
            {
                ss.UseTextInput(0);
            }
            foreach (var i in buildManager.buildDataTable[(int)selectButton.BID].useItems)
            {
                foreach (var k in iTextLIst)
                {
                    if (k.ID == (ItemID)i.Key)
                    {
                        k.UseTextInput(i.Value);
                    }
                }
            }
        }
        get
        {
            return selectButton;
        }
    }
    BuildManager buildManager;
    private void Start()
    {
        var p = GameObject.Find("Parent");
        p.GetComponentsInChildren(iTextLIst);
        createButton.onClick.AddListener(CreateButtonOnClick);
        GameObject.Find("Parent").GetComponentInChildren<PlayerItemManager>().changeTrigger += BuildButtonReload;
        buildManager = p.GetComponentInChildren<BuildManager>();
        p.GetComponentsInChildren(buildButtons);
        BuildButtonReload();
    }
    void CreateButtonOnClick()
    {
        if (buildManager.CanBuildCheck(SelectBuildButton.BID))
        {
            buildManager.BuildNum = SelectBuildButton.BID;
        }
    }
    void BuildButtonReload()
    {
        createButton.interactable = false;
        if (selectButton != null)
        {
            if (buildManager.CanBuildCheck(selectButton.BID))
            {
                createButton.interactable = true;
            }
        }

        //foreach (var i in buildButtons)
        //{
        //    if (buildManager.CanBuildCheck(i.BID))
        //    {
        //        i.button.interactable = true;
        //    }
        //    else
        //    {
        //        i.button.interactable = false;
        //    }
        //}
    }
}

