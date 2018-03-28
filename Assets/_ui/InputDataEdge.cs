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
    public EdgeChecker SelectChecker
    {
        private set
        {
            comaMane.SelectEdge.commandEdge.checker = value;
        }
        get
        {
            if (comaMane.SelectEdge == null) return null;
            return comaMane.SelectEdge.commandEdge.checker;
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
        comaMane.edgeChanged += SelectCommandEdgeChanged;
        edgeTypeDropDown.interactable = false;
    }
    //checkerの種類が変更されたら
    void EdgeTypeChanged(int num)
    {
        //nullか同じcheckertypeじゃなかったら変更。
        if (SelectChecker == null || EdgeDataBase.GetInstance().edgeDataList[num].type != SelectChecker.GetType())
        {
            SelectChecker = EdgeDataBase.GetInstance().edgeDataList[num].CreateCheckerInstance();
        }
        SetLayOut();
    }
    void SetLayOut()
    {
        foreach (var i in layOutList)
        {
            i.SetActive(false);
        }
        if (comaMane.SelectEdge != null) layOutList[edgeTypeDropDown.value].SetActive(true);
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
            edgeTypeDropDown.interactable = true;
            ApplyUI();
        }
        else
        {
            edgeTypeDropDown.interactable = false;
        }

        SetLayOut();
    }
}
