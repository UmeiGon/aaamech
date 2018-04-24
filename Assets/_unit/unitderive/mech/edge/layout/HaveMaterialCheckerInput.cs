using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class HaveMaterialCheckerInput : CheckerInput {
    [SerializeField]
    InputField itemValueInputField;
    [SerializeField]
    Dropdown itemDropDown;


    protected override void Init()
    {
        itemValueInputField.onEndEdit.AddListener(ItemValueInputChanged);
        List<string> slist = new List<string>();
        foreach (ItemID id in  Enum.GetValues(typeof(ItemID)))
        {
            slist.Add(id.NameItemID());
        }
        itemDropDown.ClearOptions();
        itemDropDown.AddOptions(slist);
        itemDropDown.value = 0;
        itemDropDown.onValueChanged.AddListener(ItemIDDropDownChanged);
        EdgeName = EdgeDataBase.GetInstance().FindNodeData<HaveMaterialEdgeChecker>().EdgeName;
    }
    protected override void ActiveTrigger()
    {
        if (inputDataToEdge.SelectChecker != null&&inputDataToEdge.SelectChecker is HaveMaterialEdgeChecker)
        {
            var checkerInstance = inputDataToEdge.SelectChecker as HaveMaterialEdgeChecker;
            itemValueInputField.text = checkerInstance.needValue.ToString();
            Debug.Log(checkerInstance.needValue);
            itemDropDown.value = checkerInstance.itemNum;
        }
    }
    void ItemIDDropDownChanged(int _num)
    {
        if (inputDataToEdge.SelectChecker is HaveMaterialEdgeChecker)
        {
            var checkerInstance = inputDataToEdge.SelectChecker as HaveMaterialEdgeChecker;
            checkerInstance.itemNum = _num;
        }
    }
    void ItemValueInputChanged(string _text)
    {
        if (inputDataToEdge.SelectChecker is HaveMaterialEdgeChecker)
        {
            var checkerInstance = inputDataToEdge.SelectChecker as HaveMaterialEdgeChecker;
            //textをintに変換してチェッカーに代入（0文字の場合は0を代入）
            if (_text.Length>0)
            {
                checkerInstance.needValue = int.Parse(_text);
            }
            else
            {
                checkerInstance.needValue = 0;
            }
        }
    }
}
