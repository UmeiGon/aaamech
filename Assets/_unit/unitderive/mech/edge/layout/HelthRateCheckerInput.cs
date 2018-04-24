using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HelthRateCheckerInput : CheckerInput {
    [SerializeField]
    InputField HelthRateInput;
    [SerializeField]
    Dropdown upDownDrop;
    protected override void Init()
    {
        HelthRateInput.onValueChanged.AddListener(ItemValueInputChanged);
        upDownDrop.onValueChanged.AddListener(DropDownChanged);
        var slist = new List<string>()
        {
            "以下",
            "以上",
        };
        upDownDrop.ClearOptions();
        upDownDrop.AddOptions(slist);
        EdgeName = EdgeDataBase.GetInstance().FindNodeData<HelthRateEdgeChecker>().EdgeName;
    }
    protected override void ActiveTrigger()
    {
        if (inputDataToEdge.SelectChecker != null&&inputDataToEdge.SelectChecker is HelthRateEdgeChecker)
        {
            var checkerInstance = inputDataToEdge.SelectChecker as HelthRateEdgeChecker;
            HelthRateInput.text = checkerInstance.helthRate.ToString();
            upDownDrop.value = checkerInstance.orMore;
        }
    }
    void DropDownChanged(int _num)
    {
        
        if (inputDataToEdge.SelectChecker is HelthRateEdgeChecker)
        {
            var checkerInstance = inputDataToEdge.SelectChecker as HelthRateEdgeChecker;
            checkerInstance.orMore = _num;
        }

    }
    void ItemValueInputChanged(string _text)
    {
        if (inputDataToEdge.SelectChecker is HelthRateEdgeChecker)
        {
            var checkerInstance = inputDataToEdge.SelectChecker as HelthRateEdgeChecker;
            if (_text.Length > 0)
            {
                checkerInstance.Par = int.Parse(_text);
                HelthRateInput.text = checkerInstance.Par.ToString();
            }
            else
            {
                checkerInstance.Par = 0;
                HelthRateInput.text = "";
            }   
        }
    }
}
