using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class AttackActivityInput : ActivityInput {

    [SerializeField]
    Dropdown matIdDrop;
    protected override void Init()
    {
        matIdDrop.ClearOptions();
        List<string> slist = new List<string>();
        foreach (CharacterType id in Enum.GetValues(typeof(CharacterType)))
        {
            slist.Add(id.NameCharacterType());
        }
        slist.Add("全ての敵");
        matIdDrop.AddOptions(slist);
        matIdDrop.onValueChanged.AddListener(PickItemIDropDownChanged);
        AcitivityName = NodeDataBase.GetInstance().FindNodeData<AttackNodeActivity>().NodeName;
    }
    protected override void ActiveTrigger()
    {
        if (inputDataToNode.SelectActivity != null && inputDataToNode.SelectActivity is AttackNodeActivity)
        {
            var atkAcitivity = inputDataToNode.SelectActivity as AttackNodeActivity;
            matIdDrop.value = atkAcitivity.targetType;
        }
    }
    void PickItemIDropDownChanged(int _num)
    {
        if (inputDataToNode.SelectActivity != null && inputDataToNode.SelectActivity is AttackNodeActivity)
        {
            var atkProgram = inputDataToNode.SelectActivity as AttackNodeActivity;
            atkProgram.targetType = _num;
        }
    }
}
