using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
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
    [SerializeField]
    PlayerItemManager pItemMane;
    [SerializeField]
    int needMechIronValue=10;
    [SerializeField]
    Button goButton;
    [SerializeField]
    Text ironText;
    [SerializeField]
    Text useText;
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
        goButton.interactable = false;
        goButton.onClick.AddListener(CreateMechInstance);
        firstChangeButton.onClick.AddListener(FirstChange);
        useText.text = needMechIronValue+"を消費してメカを作成";
        pItemMane.itemDataTable[(int)ItemID.Iron].valueChanged += GoButtonChange;
        ironText.text = pItemMane.itemDataTable[(int)ItemID.Iron].Value.ToString();
        pItemMane.itemDataTable[(int)ItemID.Iron].Value += 40;
        nowTree = new MechAITree();
    }
    public void GoButtonChange(int _value)
    {
        ironText.text = _value.ToString();
        goButton.interactable=(_value >= needMechIronValue);
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
        if (nowTree.firstCommand == null) return;
        pItemMane.itemDataTable[(int)ItemID.Iron].Value -= needMechIronValue;
        //nowTreeのcloneを渡す
        u.CreateMechInstance(nowTree);
        SaveMechAI("Inst");
        HolderReset();
        nowTree = new MechAITree();
        LoadMechAI("Inst");
    }
    public void HolderReset()
    {
        SelectEdge = null;
        SelectNode = null;
        foreach (var i in nowTree.commandList)
        {
            Destroy(i.holder.gameObject);
        }
        foreach (var i in nowTree.edgeList)
        {
            Destroy(i.holder.gameObject);
        }
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
    public void SaveMechAI(string file_name="dafault")
    {
        if (nowTree.firstCommand == null) return;
        AISaveLoad.GetInstance().SaveAITree(nowTree, file_name);
    }
    public void LoadMechAI(string file_name="default")
    {
        var d=AISaveLoad.GetInstance().LoadAITree(file_name);
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
        ////commandにedgeを対応させる
        //foreach (var i in nowTree.edgeList)
        //{
        //    i.CommandAddMe();
        //}
    }
    public void LoadCreateCommand(CommandSaveData _data,int firstId)
    {
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
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
        //programのインスタンスをゲットして,メンバ変数も入力
        if((nodeui.commandNode.program = NodeDataBase.GetInstance().nodeDataList[_data.programID].GetProgramInstance()) != null)
        {
            var fields = nodeui.commandNode.program.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(nodeui.commandNode.program,_data.programValues[i]);
            }
        }

        nowTree.commandList.Add(nodeui.commandNode);
        //firstIdだった場合色を変える
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
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //nodeClickedPopPanel.SetActive(false);
        var edge = Instantiate(commandEdgePre, Vector3.zero, new Quaternion(), commandBackPanel.transform).GetComponent<EdgeOnUI>();
        edge.cManager = this;
        edge.transform.localPosition = Vector3.zero;
        edge.commandEdge.AddPreNode(nowTree.commandList[_data.preCommandID]);
        //precheckerデータ入力
        if((edge.commandEdge.preChecker = EdgeDataBase.GetInstance().edgeDataList[_data.preCheckID].GetCheckerInstance()) != null)
        {
            var fields = edge.commandEdge.preChecker.GetType().GetFields(filedFlag);
            for (int i=0;i<fields.Length;i++)
            {
                fields[i].SetValue(edge.commandEdge.preChecker,_data.preValues[i]);
            }
        }

        edge.commandEdge.AddNextNode(nowTree.commandList[_data.nextCommandID]);
        //nextcheckerデータ入力
        if ((edge.commandEdge.nextChecker = EdgeDataBase.GetInstance().edgeDataList[_data.nextCheckID].GetCheckerInstance()) != null)
        {
            var fields = edge.commandEdge.nextChecker.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(edge.commandEdge.nextChecker, _data.nextValues[i]);
            }
        }

        edge.transform.SetAsFirstSibling();
        //treeに追加
        nowTree.edgeList.Add(edge.commandEdge);
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
