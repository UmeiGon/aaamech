using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class MaterialCheckerInput : CheckerInput {
    [SerializeField]
    InputField itemValueInputField;
    [SerializeField]
    Dropdown itemDropDown;
    MaterialChecker checkerInstance;


    private void Awake()
    {
        base.Awake();
        itemValueInputField.onEndEdit.AddListener(ItemValueInputChanged);
        List<string> slist = new List<string>();
        foreach (ItemID id in  Enum.GetValues(typeof(ItemID)))
        {
            slist.Add(id.NameItemID());
        }
        itemDropDown.AddOptions(slist);
        itemDropDown.value = 0;
        itemDropDown.onValueChanged.AddListener(ItemIDDropDownChanged);
    }
    private void OnEnable()
    {
        if (ide.SelectChecker is MaterialChecker)
        {
            checkerInstance = ide.SelectChecker as MaterialChecker;
            itemValueInputField.text = checkerInstance.needValue.ToString();
            itemDropDown.value = (int)checkerInstance.itemNum;
        }
        else
        {
            itemValueInputField.text = "バグってます";
        }  
    }
    void ItemIDDropDownChanged(int _num)
    {
        checkerInstance.itemNum = (ItemID)_num;
    }
    void ItemValueInputChanged(string _text)
    {
        checkerInstance.needValue = int.Parse(_text);
    }
}
