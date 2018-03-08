using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
public class UICommandManager : MonoBehaviour
{
    [SerializeField]
    Canvas myCanvas;
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
        StartCoroutine(KeyControllUpdate());
    }
    IEnumerator KeyControllUpdate()
    {
        while (true)
        {
            if (myCanvas.enabled)
            {
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    DeleteSelectEdge();
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    DeleteSelectEdge();
                }
            }
            yield return null;
        }
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
    void AloneDeleteEdge()
    {
        var alones = nowTree.edgeList.FindAll(x=>x.pre==null&&x.next==null);
        foreach (var i in alones)
        {
            DeleteEdge(i.holder);
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
            if (SelectNode.commandNode == nowTree.firstCommand)
            {
                if(nowTree.commandList.Count > 0)
                {
                    nowTree.firstCommand = nowTree.commandList[0];
                    FirstCommandColorChange(null, nowTree.firstCommand);
                }
                else
                {
                    nowTree.firstCommand = null;
                }
               
            }
            SelectNode.DeleteNode();
            Destroy(SelectNode.gameObject);
            SelectNode = null;
            AloneDeleteEdge();
        }
    }
    public void CreateCommandNode(Vector3 pos)
    {

        var c = Instantiate(commandNodePre, new Vector3(0, 0, 0), new Quaternion(), commandBackPanel.transform);
        c.transform.position = pos;
        c.transform.SetAsLastSibling();
        var nodeui = c.GetComponent<NodeOnUI>();
        CommandInit(nodeui);
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
        SelectNode = nodeui;
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
            i.holder.DeleteNode();
            Destroy(i.holder.gameObject);
        }
        foreach (var i in nowTree.edgeList)
        {
            i.holder.DeleteEdge();
            Destroy(i.holder.gameObject);
        }
        nowTree = new MechAITree();
    }
    public void SaveMechAI(string file_name="dafault")
    {
        if (nowTree.firstCommand == null) return;
        AISaveLoad.GetInstance().SaveAITree(AIPackage.TreeToPackage(nowTree), file_name);
    }
    public void LoadMechAI(string file_name="default")
    {
        var d=AISaveLoad.GetInstance().LoadAITree(file_name);
        if (d == null) return;
        TreeReset();
        foreach (var c in d.commandDataList)
        {
            LoadCreateCommand(c, d.firstCommandID);
        }
        foreach (var e in d.edgeDataList)
        {
            LoadCreateEdge(e);
        }
        ////commandにedgeを対応させる
        //foreach (var i in nowTree.edgeList)
        //{
        //    i.CommandAddMe();
        //}
    }
    void CommandInit(NodeOnUI node_ui)
    {
        node_ui.commandNode.holder = node_ui;
        node_ui.cManager = this;
        //リミット付ける
        node_ui.startLimitPosition = new Vector3(0, 0, 0);
        node_ui.endLimitPosition = commandBackPanel.GetComponent<RectTransform>().sizeDelta;
        node_ui.startLimitPosition.y *= -1;
        node_ui.endLimitPosition.y *= -1;
        node_ui.commandNode.program = NodeDataBase.GetInstance().nodeDataList[0].GetProgramInstance();
        nowTree.commandList.Add(node_ui.commandNode);
    }
    public void LoadCreateCommand(CommandSaveData _data,int firstId)
    {
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var c = Instantiate(commandNodePre, new Vector3(0, 0, 0), new Quaternion(), commandBackPanel.transform);
        c.transform.localPosition = _data.localPos;
        c.transform.SetAsLastSibling();
        var nodeui = c.GetComponent<NodeOnUI>();
        CommandInit(nodeui);
        //programのインスタンスをゲットして,メンバ変数も入力
        if((nodeui.commandNode.program = NodeDataBase.GetInstance().nodeDataList[_data.programID].GetProgramInstance()) != null)
        {
            var fields = nodeui.commandNode.program.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(nodeui.commandNode.program,_data.programValues[i]);
            }
        }
        //firstIdだった場合色を変える
        if (_data.commandNumber==firstId)
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
        edge.commandEdge.holder = edge;
        edge.commandEdge.AddPreNode(nowTree.commandList[_data.preCommandNumber]);
 

        edge.commandEdge.AddNextNode(nowTree.commandList[_data.nextCommandNumber]);
        //nextcheckerデータ入力
        if ((edge.commandEdge.checker = EdgeDataBase.GetInstance().edgeDataList[_data.checkrID].GetCheckerInstance()) != null)
        {
            var fields = edge.commandEdge.checker.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(edge.commandEdge.checker, _data.checkerValues[i]);
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
    public void DeleteSelectEdge()
    {
        DeleteEdge(SelectEdge);
    }
    public void DeleteEdge(EdgeOnUI _edge)
    {
        if (_edge)
        {
            nowTree.edgeList.Remove(_edge.commandEdge);
            _edge.DeleteEdge();
            Destroy(_edge.gameObject);
            if (SelectEdge== _edge) SelectEdge= null;
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
