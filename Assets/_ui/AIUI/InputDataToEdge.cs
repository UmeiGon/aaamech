using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputDataToEdge : MonoBehaviour
{
    [SerializeField]
    Dropdown edgeTypeDropDown;
    [SerializeField]
    Text edgeDescriptionText;
    AITreeGenerator aiGenerator;
   
    Dictionary<string,CheckerInput> edgeLayOutDictionary=new Dictionary<string, CheckerInput>();
    public EdgeChecker SelectChecker
    {
        private set
        {
            aiGenerator.SelectEdge.commandEdge.checker = value;
        }
        get
        {
            if (aiGenerator==null)
            {
                return null;
            }
           return(aiGenerator.SelectEdge)?aiGenerator.SelectEdge.commandEdge.checker:null;
        }
    }
    public void AddEdgeLayout(string edge_name,CheckerInput checker_input)
    {
        if (!edgeLayOutDictionary.ContainsKey(edge_name))
        {
            edgeLayOutDictionary.Add(edge_name, checker_input);
        }
    }
    private void Awake()
    {
        aiGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
    }
    private void Start()
    {
        //edgeの種類分だけ名前をドロップダウンに追加
        List<string> slist = new List<string>();
        foreach (var s in EdgeDataBase.GetInstance().EdgeDataList)
        {
            slist.Add(s.EdgeName);
        }
        edgeTypeDropDown.ClearOptions();
        edgeTypeDropDown.AddOptions(slist);
        edgeTypeDropDown.onValueChanged.AddListener(EdgeTypeChanged);
        aiGenerator.edgeChanged += SelectEdgeChanged;
        edgeTypeDropDown.interactable = false;
        FixLayOut();
    }
    //checkerの種類が変更されたら
    void EdgeTypeChanged(int num)
    {
        //nullか同じcheckertypeじゃなかったら変更。
        if (SelectChecker == null || EdgeDataBase.GetInstance().EdgeDataList[num].CheckerType != SelectChecker.GetType())
        {
            SelectChecker = EdgeDataBase.GetInstance().EdgeDataList[num].CreateCheckerInstance();
        }
        FixLayOut();
    }
    void FixLayOut()
    {
        int checkerNum = edgeTypeDropDown.value;
        foreach (var i in edgeLayOutDictionary)
        {
            if (i.Key==EdgeDataBase.GetInstance().EdgeDataList[checkerNum].EdgeName)
            {
                edgeDescriptionText.text = EdgeDataBase.GetInstance().EdgeDataList[checkerNum].EdgeDescription;
                i.Value.LayoutSetActive(true);
            }
            else
            {
                i.Value.LayoutSetActive(false);
            }
        }
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
    void SelectEdgeChanged()
    {
        //選択edgeがnullじゃない場合
        if (aiGenerator.SelectEdge != null)
        {
            edgeTypeDropDown.interactable = true;
            ApplyUI();
        }
        else
        {
            edgeTypeDropDown.interactable = false;
        }

        FixLayOut();
    }
}
