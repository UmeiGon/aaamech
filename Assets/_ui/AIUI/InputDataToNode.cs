using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputDataToNode : MonoBehaviour {
    AITreeGenerator AiGenerator;
    [SerializeField]
    Dropdown activityTypeDrop;
    [SerializeField]
    Text nodeDescriptionText;
    Dictionary<string,ActivityInput> nodeLayOutDictionary=new Dictionary<string, ActivityInput>();
    public NodeActivity SelectActivity{
        private set
        {
            if(AiGenerator.SelectNode)AiGenerator.SelectNode.commandNode.activity = value;
        }
        get
        {
            if (AiGenerator==null)
            {
                return null;
            }
            return (AiGenerator.SelectNode)?AiGenerator.SelectNode.commandNode.activity:null;
        }
    }
    public void AddNodeLayOut(string node_name,ActivityInput activity_input)
    {
        if (!nodeLayOutDictionary.ContainsKey(node_name)) 
        {
              nodeLayOutDictionary.Add(node_name, activity_input);
        }
    }
    private void Awake()
    {
        AiGenerator = CompornentUtility.FindCompornentOnScene<AITreeGenerator>();
    }
    private void Start()
    {
        List<string> slist = new List<string>();
        foreach (var i in NodeDataBase.GetInstance().NodeDataList)
        {
            slist.Add(i.NodeName);
        }
        activityTypeDrop.ClearOptions();
        activityTypeDrop.AddOptions(slist);
        activityTypeDrop.value = 0;
        activityTypeDrop.onValueChanged.AddListener(NodeTypeChanged);
        AiGenerator.activityChanged += SelectActivityChanged;
        activityTypeDrop.interactable = false;
        FixLayOut();
    }
    void FixLayOut()
    {
        int actNum = activityTypeDrop.value;
        foreach (var i in nodeLayOutDictionary)
        {
            if (i.Key==NodeDataBase.GetInstance().NodeDataList[actNum].NodeName)
            {
                nodeDescriptionText.text = NodeDataBase.GetInstance().NodeDataList[actNum].NodeDescription;
                i.Value.LayoutSetActive(true);
            }
            else
            {
                i.Value.LayoutSetActive(false);
            }
        }
    }
 
    
    void NodeTypeChanged(int _num)
    {
        //ノードタイプが変更されるたびに新しいインスタンスを生成
        if (SelectActivity == null|| NodeDataBase.GetInstance().NodeDataList[_num].ActivityType != SelectActivity.GetType())
        {
            SelectActivity = NodeDataBase.GetInstance().NodeDataList[_num].GetActivityInstance();
        }
        FixLayOut();
    }
    void ApplyCommandType()
    {
        if (SelectActivity == null)
        {
            activityTypeDrop.value = 0;
        }
        else
        {
            activityTypeDrop.value = NodeDataBase.GetInstance().FindTypeNumber(SelectActivity);
        }
    }
    void SelectActivityChanged()
    {
        if (AiGenerator.SelectNode!=null)
        {
            activityTypeDrop.interactable = true;
            ApplyCommandType();
        }
        else
        { 
            activityTypeDrop.interactable = false;
        }
        FixLayOut();
    }
}
