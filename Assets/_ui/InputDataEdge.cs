using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputDataEdge : MonoBehaviour
{
    [SerializeField]
    Dropdown edgeTypeDropDown;
    [SerializeField]
    UICommandManager comaMane;
    [SerializeField]
    List<GameObject> layOutList;
    [SerializeField]
    Dropdown checkerStreamDrop;
    bool selectIsNext=true;
    public EdgeChecker SelectChecker
    {
        private set
        {
            if (selectIsNext)
            {
                comaMane.SelectEdge.commandEdge.nextChecker = value;
            }
            else
            {
                comaMane.SelectEdge.commandEdge.preChecker = value;
            }
        }
        get
        {
            if (comaMane.SelectEdge == null) return null;
            if (selectIsNext)
            {
                return comaMane.SelectEdge.commandEdge.nextChecker;
            }
            else
            {
                return comaMane.SelectEdge.commandEdge.preChecker;
            }
        }
    }
    private void Start()
    {
        //edgeの種類分だけ名前をドロップダウンに追加
        List<string> slist = new List<string>();
        foreach (var s in EdgeDataBase.GetInstance().edgeDataList)
        {
            slist.Add(s.edgeName);
        }
        edgeTypeDropDown.ClearOptions();
        edgeTypeDropDown.AddOptions(slist);
        edgeTypeDropDown.onValueChanged.AddListener(EdgeTypeChanged);
        checkerStreamDrop.onValueChanged.AddListener(CheckerStreamChanged);
        comaMane.edgeChanged += SelectCommandEdgeChanged;
        List<string> nplist = new List<string>
        {
            "正方向",
            "逆方向"
        };
        checkerStreamDrop.ClearOptions();
        checkerStreamDrop.AddOptions(nplist);
        checkerStreamDrop.value = 0;
        edgeTypeDropDown.interactable = false;
        checkerStreamDrop.interactable = false;
    }
    void CheckerStreamChanged(int _num)
    {
        if (comaMane.SelectEdge == null) return;
        switch (_num)
        {
            case 0:
                selectIsNext = true;
                Debug.Log("next");
                break;
            case 1:
                selectIsNext = false;
                Debug.Log("pre");
                break;
        }
        if (SelectChecker != null) Debug.Log(SelectChecker.GetType());
        ApplyUI();
    }
    //checkerの種類が変更されたら
    void EdgeTypeChanged(int num)
    {
        //nullか同じcheckertypeじゃなかったら変更。
        if (SelectChecker == null|| EdgeDataBase.GetInstance().edgeDataList[num].type != SelectChecker.GetType())
        {
            SelectChecker = EdgeDataBase.GetInstance().edgeDataList[num].GetCheckerInstance();
        }
        SetLayOut();
    }
    void SetLayOut()
    {
        foreach (var i in layOutList)
        {
            i.SetActive(false);
        }
        if(comaMane.SelectEdge!=null)layOutList[edgeTypeDropDown.value].SetActive(true);
    }
    //selectcheckerの情報をUIに適用
    void ApplyUI()
    {
        if (SelectChecker == null)
        {
            //nullは0
            edgeTypeDropDown.value = 0;

        }
        else
        {
            edgeTypeDropDown.value = EdgeDataBase.GetInstance().FindTypeNumber(SelectChecker);
        }
    }
    void SelectCommandEdgeChanged()
    {
        //選択edgeがnullじゃない場合
        if (comaMane.SelectEdge != null)
        {
            checkerStreamDrop.interactable = true;
            edgeTypeDropDown.interactable = true;
            ApplyUI();
        }
        else
        {
            checkerStreamDrop.interactable = false;
            edgeTypeDropDown.interactable = false;
        }

        SetLayOut();
    }
}
