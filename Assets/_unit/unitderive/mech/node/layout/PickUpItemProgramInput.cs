using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class PickUpItemProgramInput : ProgramInput
{

    [SerializeField]
    Dropdown matIdDrop;
    private void Start()
    {
        matIdDrop.ClearOptions();
        List<string> slist = new List<string>();
        foreach (MaterialID id in Enum.GetValues(typeof(MaterialID)))
        {
            slist.Add(id.NameMaterialID());
        }
        matIdDrop.AddOptions(slist);
        matIdDrop.onValueChanged.AddListener(PickItemIDropDownChanged);
    }
    private void OnEnable()
    {
        if (idc.SelectProgram is PickUpItemProgram)
        {
            var pickProgram = idc.SelectProgram as PickUpItemProgram;
            matIdDrop.value = pickProgram.pickItemID;
        }
    }
    void PickItemIDropDownChanged(int _num)
    {
        if (idc.SelectProgram is PickUpItemProgram)
        {
            var a = idc.SelectProgram as PickUpItemProgram;
            a.pickItemID = _num;
        }
    }
}
