using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class AttackProgramInput : ProgramInput {

    [SerializeField]
    Dropdown matIdDrop;
    private void Start()
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
    }
    private void OnEnable()
    {
        if (idc.SelectProgram is AttackProgram)
        {
            var atkProgram = idc.SelectProgram as AttackProgram;
            matIdDrop.value = atkProgram.targetType;
        }
    }
    void PickItemIDropDownChanged(int _num)
    {
        if (idc.SelectProgram is AttackProgram)
        {
            var atkProgram = idc.SelectProgram as AttackProgram;
            atkProgram.targetType = _num;
        }
    }
}
