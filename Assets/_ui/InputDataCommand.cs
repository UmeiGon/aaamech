using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputDataCommand : MonoBehaviour {
    [SerializeField]
    UICommandManager comaMane;
    [SerializeField]
    Dropdown commandTypeDrop;
    [SerializeField]
    List<GameObject> layOutList;
    public CommandProgram SelectProgram{
        private set
        {
            if(comaMane.SelectNode)comaMane.SelectNode.commandNode.program = value;
        }
        get
        {
            return (comaMane.SelectNode)?comaMane.SelectNode.commandNode.program:null;
        }
    }
    private void Start()
    {
        List<string> slist = new List<string>();
        foreach (var i in NodeDataBase.GetInstance().nodeDataList)
        {
            slist.Add(i.nodeName);
        }
        commandTypeDrop.AddOptions(slist);
        commandTypeDrop.value = 0;
        commandTypeDrop.onValueChanged.AddListener(NodeTypeChanged);
        comaMane.commandChanged += SelectCommandChanged;
    }
    void SetLayOut()
    {
        foreach (var i in layOutList)
        {
            i.SetActive(false);
        }
        layOutList[commandTypeDrop.value].SetActive(true);
    }
 
    //ノードタイプが変更されるたびに新しいインスタンスを生成
    void NodeTypeChanged(int _num)
    {
        if (SelectProgram == null|| NodeDataBase.GetInstance().nodeDataList[_num].type != SelectProgram.GetType())
        {
            SelectProgram = NodeDataBase.GetInstance().nodeDataList[_num].GetProgramInstance();
        }
 
        
        SetLayOut();
    }
    void ApplyCommandType()
    {
        if (SelectProgram == null)
        {
            commandTypeDrop.value = 0;
        }
        else
        {
            commandTypeDrop.value = NodeDataBase.GetInstance().FindTypeNumber(SelectProgram);
        }
    }
    void SelectCommandChanged()
    {
        if (comaMane.SelectNode!=null)
        {
            commandTypeDrop.interactable = true;
            ApplyCommandType();
        }
        else
        { 
            commandTypeDrop.interactable = false;
        }
        SetLayOut();
    }
}
