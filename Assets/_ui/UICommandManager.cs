using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICommandManager : MonoBehaviour
{

    [SerializeField]
    GameObject commandBackPanel;
    [SerializeField]
    GameObject commandNodePre;
    [SerializeField]
    GameObject commandEdgePre;
    [SerializeField]
    Button createEdgeButton;
    [SerializeField]
    Button deleteEdgeButton;
    [SerializeField]
   Button deleteNodeButton;
    [SerializeField]
    Button firstChangeButton;
    [SerializeField]
    Color defaultNodeColor;
    [SerializeField]
    Color firstNodeColor;
 
    MechAITree nowTree = null;
    //実態こっちは使用してはいけない
    EdgeOnUI selectEdgeReal;
    NodeOnUI selectNodeReal;
    public delegate void SelectEdgeChanged();
    public SelectEdgeChanged edgeChanged;
    public delegate void SelectCommandChanged();
    public SelectCommandChanged commandChanged;
    private void Start()
    {
        createEdgeButton.interactable = false;
        firstChangeButton.interactable = false;
        deleteEdgeButton.interactable = false;
        deleteNodeButton.interactable = false;
        firstChangeButton.onClick.AddListener(FirstChange);
        nowTree = new MechAITree();
    }
    public EdgeOnUI SelectEdge
    {
        get { return selectEdgeReal; }
        set
        {
            if (selectEdgeReal != null)
            {
                selectEdgeReal.NonSelectTrigger();
            }
            selectEdgeReal = value;
            if (selectEdgeReal != null)
            {
                selectEdgeReal.SelectTrigger();
                SelectedEdgeTrigger();
            }
            else
            {
                NotSelectedEdgeTrigger();
            }
            if (edgeChanged != null) edgeChanged();
        }
    }
    public NodeOnUI SelectNode
    {
        get { return selectNodeReal; }
        set
        {
            if (selectNodeReal != null)
            {
                selectNodeReal.NonSelectTrigger();
            }
            selectNodeReal = value;
            if (selectNodeReal != null)
            {
                selectNodeReal.SelectTrigger();
                SelectedNodeTrigger();
            }
            else
            {
                NotSelectedNodeTrigger();
            }
            if (commandChanged != null) commandChanged();
        }
    }
    public void FirstChange()
    {
        if (SelectNode != null)
        {
            FirstCommandColorChange(nowTree.firstCommand,SelectNode.commandNode);
            nowTree.firstCommand = SelectNode.commandNode;
        }
    }
    public void FirstCommandColorChange(Command pre_first_command, Command now_first_command)
    {
        if (pre_first_command != null) pre_first_command.holder.GetComponent<Image>().color = defaultNodeColor;
        if (now_first_command != null) now_first_command.holder.GetComponent<Image>().color = firstNodeColor;
    }
    public void DeleteCommandNode()
    {
        if (SelectNode != null)
        {
            nowTree.commandList.Remove(SelectNode.commandNode);
            if (SelectNode.commandNode == nowTree.firstCommand&&nowTree.commandList.Count>0)
            {
                nowTree.firstCommand = nowTree.commandList[0];
                FirstCommandColorChange(null, nowTree.firstCommand);
            }    
            SelectNode.DeleteNode();
            Destroy(SelectNode.gameObject);
            SelectNode = null;
        }
    }
    public void CreateCommandNode(Vector3 pos)
    {

        var c = Instantiate(commandNodePre, new Vector3(0, 0, 0), new Quaternion(), commandBackPanel.transform);
        c.transform.position = pos;
        c.transform.SetAsLastSibling();
        var nodeui = c.GetComponent<NodeOnUI>();
        nodeui.commandNode.holder = nodeui.transform;
        nodeui.cManager = this;
        nodeui.startLimitPosition = new Vector3(0, 0, 0);
        nodeui.endLimitPosition = commandBackPanel.GetComponent<RectTransform>().sizeDelta;
        nodeui.startLimitPosition.y *= -1;
        nodeui.endLimitPosition.y *= -1;
        nowTree.commandList.Add(nodeui.commandNode);
        //firstがまだ設定されて無かったらfirstに
        if (nowTree.firstCommand == null)
        {
            nowTree.firstCommand = nodeui.commandNode;
            FirstCommandColorChange(null,nodeui.commandNode);
        }
        else
        {
            nodeui.GetComponent<Image>().color = defaultNodeColor;
        }

    }
    [SerializeField] UIMechManager u;
    public void CreateMechInstance()
    {
        u.CreateMechInstance(nowTree);
    }
    public void TreeReset()
    {
        SelectEdge = null;
        SelectNode = null;
        foreach (var i in nowTree.commandList)
        {
            i.holder.GetComponent<NodeOnUI>().DeleteNode();
            Destroy(i.holder.gameObject);
        }
        foreach (var i in nowTree.edgeList)
        {
            i.holder.GetComponent<EdgeOnUI>().DeleteEdge();
            Destroy(i.holder.gameObject);
        }
        nowTree = new MechAITree();
    }
    public void LoadMechAI()
    {
        var d=AISaveLoad.GetInstance().LoadAITree();
        if (d == null) return;
        TreeReset();
        foreach (var c in d.commandSaveList)
        {
            LoadCreateCommand(c, d.firstCommandID);
        }
        foreach (var e in d.edgeSaveList)
        {
            LoadCreateEdge(e);
        }
        
    }
    public void LoadCreateCommand(CommandSaveData _data,int firstId)
    {
        var c = Instantiate(commandNodePre, new Vector3(0, 0, 0), new Quaternion(), commandBackPanel.transform);
        c.transform.localPosition = _data.localPos;
        c.transform.SetAsLastSibling();
        var nodeui = c.GetComponent<NodeOnUI>();
        nodeui.commandNode.holder = nodeui.transform;
        nodeui.cManager = this;
        //リミット付ける
        nodeui.startLimitPosition = new Vector3(0, 0, 0);
        nodeui.endLimitPosition = commandBackPanel.GetComponent<RectTransform>().sizeDelta;
        nodeui.startLimitPosition.y *= -1;
        nodeui.endLimitPosition.y *= -1;

        nodeui.commandNode.program = NodeDataBase.GetInstance().nodeDataList[_data.programID].GetProgramInstance();
        nowTree.commandList.Add(nodeui.commandNode);
        //firstがまだ設定されて無かったらfirstに
        if (_data.commandID==firstId)
        {
            nowTree.firstCommand = nodeui.commandNode;
            FirstCommandColorChange(null, nodeui.commandNode);
        }
        else
        {
            nodeui.GetComponent<Image>().color = defaultNodeColor;
        }
    }
    public void LoadCreateEdge(EdgeSaveData _data)
    {
        //nodeClickedPopPanel.SetActive(false);
        var edge = Instantiate(commandEdgePre, Vector3.zero, new Quaternion(), commandBackPanel.transform).GetComponent<EdgeOnUI>();
        edge.cManager = this;
        edge.transform.localPosition = Vector3.zero;
        edge.commandEdge.AddPreNode(nowTree.commandList[_data.preCommandID]);
        edge.commandEdge.preChecker = EdgeDataBase.GetInstance().edgeDataList[_data.preCheckID].GetCheckerInstance();
        edge.commandEdge.AddNextNode(nowTree.commandList[_data.nextCommandID]);
        edge.commandEdge.nextChecker = EdgeDataBase.GetInstance().edgeDataList[_data.nextCheckID].GetCheckerInstance();
        edge.transform.SetAsFirstSibling();
        //treeに追加
        nowTree.edgeList.Add(edge.commandEdge);
    }
    public void SaveMechAI()
    {
        AISaveLoad.GetInstance().SaveAITree(nowTree);
        //gameObject.SetActive(false);
    }
    void SelectedEdgeTrigger()
    {
        deleteEdgeButton.interactable = true;
    }
    void NotSelectedEdgeTrigger()
    {
        deleteEdgeButton.interactable = false;
    }
    void SelectedNodeTrigger()
    {
        firstChangeButton.interactable = true;
        createEdgeButton.interactable = true;
        deleteNodeButton.interactable = true;
    }
    void NotSelectedNodeTrigger()
    {
        firstChangeButton.interactable = false;
        createEdgeButton.interactable = false;
        deleteNodeButton.interactable = false;
    }
    public void NodeRightClick(NodeOnUI _node)
    {
        SelectNode = _node;
    }
    public void NodeLeftClick(NodeOnUI _node)
    {
        //nodeにselectedgeを付ける
        if (SelectEdge)
        {
            if (!SelectEdge.commandEdge.AddNextNode(_node.commandNode))
            {
                SelectEdge.commandEdge.AddPreNode(_node.commandNode);
            }
        }
    }
    public void DeleteCommandEdge()
    {
        if (SelectEdge)
        {
            nowTree.edgeList.Remove(SelectEdge.commandEdge);
            SelectEdge.DeleteEdge();
            Destroy(SelectEdge.gameObject);
            SelectEdge = null;
        }
    }
    public void CreateCommandEdge()
    {

        if (SelectNode)
        {
            //nodeClickedPopPanel.SetActive(false);
            SelectEdge = Instantiate(commandEdgePre, Vector3.zero, new Quaternion(), commandBackPanel.transform).GetComponent<EdgeOnUI>();
            SelectEdge.cManager = this;
            SelectEdge.transform.localPosition = Vector3.zero;
            SelectEdge.commandEdge.AddPreNode(SelectNode.commandNode);
            SelectEdge.transform.SetAsFirstSibling();
            //treeに追加
            nowTree.edgeList.Add(SelectEdge.commandEdge);
        }
    }

}
