using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class PickUpItemActivityInput : ActivityInput
{
    [SerializeField]
    Dropdown matIdDrop;
  
    protected override void Init()
    {
        AcitivityName = NodeDataBase.GetInstance().FindNodeData<PickUpItemNodeActivity>().NodeName;

        matIdDrop.ClearOptions();
        List<string> slist = new List<string>();
        foreach (MaterialID id in Enum.GetValues(typeof(MaterialID)))
        {
            slist.Add(id.NameMaterialID());
        }
        matIdDrop.AddOptions(slist);
        matIdDrop.onValueChanged.AddListener(PickItemIDropDownChanged); 
    }
    protected override void ActiveTrigger()
    {
        if (inputDataToNode.SelectActivity!=null && inputDataToNode.SelectActivity is PickUpItemNodeActivity)
        {
            var pickProgram = inputDataToNode.SelectActivity as PickUpItemNodeActivity;
            matIdDrop.value = pickProgram.pickItemID;
        }
    }
    void PickItemIDropDownChanged(int _num)
    {
        if (inputDataToNode.SelectActivity != null && inputDataToNode.SelectActivity is PickUpItemNodeActivity)
        {
            var a = inputDataToNode.SelectActivity as PickUpItemNodeActivity;
            a.pickItemID = _num;
        }
    }
}
