using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
public class AITreeGenerator : MonoBehaviour
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
    Button resetButton;
    [SerializeField]
    Button saveButton;
    [SerializeField]
    Button loadButton;
    [SerializeField]
    GameObject blockingEditMask;
    MechGenerator mechGenerator;
    public MechAITree editingTree;
    //実態こっちは使用してはいけない
    EdgeOnUI selectEdgeReal;
    NodeOnUI selectNodeReal;
    bool canEdit;
    public bool CanEdit
    {
        get
        {
            return canEdit;
        }
        set
        {
            canEdit = value;
            blockingEditMask.SetActive(!canEdit);
        }
    }
    public Action edgeChanged;
    public Action activityChanged;
    private void Start()
    {
        createEdgeButton.interactable = false;
        createEdgeButton.onClick.AddListener(CreateDefaultEdge);
        firstChangeButton.interactable = false;
        firstChangeButton.onClick.AddListener(ChangeFirstNode);
        deleteEdgeButton.interactable = false;
        deleteEdgeButton.onClick.AddListener(DeleteSelectEdge);
        deleteNodeButton.interactable = false;
        deleteNodeButton.onClick.AddListener(DeleteSelectNode);
        resetButton.onClick.AddListener(ResetEditingTree);
        mechGenerator = CompornentUtility.FindCompornentOnScene<MechGenerator>();
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
            }
            yield return null;
        }
    }

    public EdgeOnUI SelectEdge
    {
        get { return selectEdgeReal; }
        set
        {
            if (selectEdgeReal != null)
            {
                selectEdgeReal.UnSelectTrigger();
            }
            selectEdgeReal = value;
            if (selectEdgeReal != null)
            {
                selectEdgeReal.SelectTrigger();
                SelectedEdgeTrigger();
            }
            else
            {
                UnSelectedEdgeTrigger();
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
                UnSelectedNodeTrigger();
            }
            if (activityChanged != null) activityChanged();
        }
    }
    void SucideEdgeIfAlone()
    {
        var alones = editingTree.edgeList.FindAll(x => x.pre == null && x.next == null);
        foreach (var i in alones)
        {
            DeleteEdge(i.holder);
        }
    }
    public void ChangeFirstNode()
    {
        if (SelectNode != null)
        {
            ChangeNodeColor(editingTree.firstNode.holder, firstNodeColor);
            ChangeNodeColor(SelectNode.CommandNode.holder, defaultNodeColor);
            editingTree.firstNode = SelectNode.CommandNode;
        }
    }

    void ChangeNodeColor(NodeOnUI firstCommand, Color _color)
    {
        firstCommand.NodeColor = _color;
    }

    public void GenerateMech()
    {
        if (editingTree==null||editingTree.firstNode == null) return;
        var copyMech = PackageConvertToAITree(AIPackage.AITreeConvertToPackage(editingTree), true);
        mechGenerator.GenerateMech(copyMech);
        myCanvas.enabled = false;
    }
    public void ResetEditingTree()
    {
        SelectEdge = null;
        SelectNode = null;
        foreach (var i in editingTree.nodeList)
        {
            i.holder.Sucide();
        }
        foreach (var i in editingTree.edgeList)
        {
            i.holder.Sucide();
        }
    }
    public void SaveEditingMechAI(string file_name = "dafault")
    {
        SaveMechAI(editingTree, file_name);
    }
    public void SaveMechAI(MechAITree ai_tree, string file_name = "dafault")
    {
        if (ai_tree.firstNode == null) return;
        AISaveLoad.GetInstance().SaveAITree(AIPackage.AITreeConvertToPackage(ai_tree), file_name);
    }
    public MechAITree LoadMechAI(string file_name = "default")
    {
        return PackageConvertToAITree(AISaveLoad.GetInstance().LoadAITree(file_name));
    }

    //AIPackageをAITreeに変換
    MechAITree PackageConvertToAITree(AIPackage ai_pack, bool IsAITreeOnly = false)
    {
        if (ai_pack == null) return null;
        MechAITree retAI = new MechAITree();
        if (ai_pack == null) return retAI;
        int n = 0;
        foreach (var nodeData in ai_pack.nodeDataList)
        {
            var node = LoadCommandNode(nodeData, IsAITreeOnly);
            retAI.nodeList.Add(node);
            //firstNodeの時の設定
            if (ai_pack.firstCommandID == n)
            {
                retAI.firstNode = node;
                if (!IsAITreeOnly) ChangeNodeColor(retAI.firstNode.holder, firstNodeColor);
            }
            n++;
        }
        foreach (var edgeData in ai_pack.edgeDataList)
        {
            var edge = LoadCommandEdge(edgeData, retAI, IsAITreeOnly);
            retAI.edgeList.Add(edge);
        }
        return retAI;
    }

    void SelectedEdgeTrigger()
    {
        deleteEdgeButton.interactable = true;
    }
    void UnSelectedEdgeTrigger()
    {
        deleteEdgeButton.interactable = false;
    }
    void SelectedNodeTrigger()
    {
        firstChangeButton.interactable = true;
        createEdgeButton.interactable = true;
        deleteNodeButton.interactable = true;
    }
    void UnSelectedNodeTrigger()
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
            if (!SelectEdge.CommandEdge.AddNextNode(_node.CommandNode))
            {
                SelectEdge.CommandEdge.AddPreNode(_node.CommandNode);
            }
        }
    }
    //画面範囲外にノードが行かないように制限
    void SetLimitNode(NodeOnUI node_ui)
    {
        node_ui.startLimitPosition = new Vector3(0, 0, 0);
        node_ui.endLimitPosition = commandBackPanel.GetComponent<RectTransform>().sizeDelta;
        node_ui.startLimitPosition.y *= -1;
        node_ui.endLimitPosition.y *= -1;
    }

    //UI上でのnodeを作成
    NodeOnUI CreateNodeOnUI(Vector3 _pos, bool is_worldpos)
    {
        var nodeUI = Instantiate(commandNodePre, new Vector3(0, 0, 0), Quaternion.identity, commandBackPanel.transform).GetComponent<NodeOnUI>();
        if (is_worldpos)
        {
            nodeUI.transform.position = _pos;
        }
        else
        {
            nodeUI.transform.localPosition = _pos;
        }
        ChangeNodeColor(nodeUI, defaultNodeColor);
        nodeUI.transform.SetAsLastSibling();
        nodeUI.aITreeGenerator = this;
        //画面範囲外に行けないようにリミット付ける
        SetLimitNode(nodeUI);
        return nodeUI;
    }
    //AIのnodeを作成
    CommandNode CreateCommandNode(int acitivity_id = 0, List<int> acitivity_values = null)
    {
        var node = new CommandNode
        {
            activity = NodeDataBase.GetInstance().CreateAcitivityInstance(acitivity_id)
        };
        if (node.activity != null && acitivity_values != null)
        {
            var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var fields = node.activity.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(node.activity, acitivity_values[i]);
            }
        }
        return node;
    }
    //UI上でのnodeとAIのnodeを作成
    CommandNode CreateNodeAndUI(Vector3 _pos, bool world_pos_flag, int acitivity_id = 0, List<int> acitivity_values = null)
    {
        var nodeUI = CreateNodeOnUI(_pos, world_pos_flag);
        nodeUI.CommandNode = CreateCommandNode(acitivity_id, acitivity_values);
        return nodeUI.CommandNode;
    }
    void DeleteNode(NodeOnUI _node)
    {
        if (_node == null) return;
        editingTree.nodeList.Remove(_node.CommandNode);
        //消すノードがファーストノードだった時の設定
        if (_node.CommandNode == editingTree.firstNode)
        {
            if (editingTree.nodeList.Count > 0)
            {
                editingTree.firstNode = editingTree.nodeList[0];
                ChangeNodeColor(editingTree.firstNode.holder, firstNodeColor);
            }
            else
            {
                editingTree.firstNode = null;
            }

        }

        _node.Sucide();
        SucideEdgeIfAlone();
    }
    public void DeleteSelectNode()
    {
        DeleteNode(SelectNode);
    }
    public void CreateDefaultNode(Vector3 _pos)
    {
        CommandNode node;
        if (TutorialSetter.IsTutorial)
        {
            node = CreateNodeAndUI(_pos, true, 0, new List<int> { 1 });   
        }
        else
        {
            node = CreateNodeAndUI(_pos, true);
        }
        //firstがまだ設定されて無かったらfirstに
        if (editingTree.firstNode == null)
        {
            editingTree.firstNode = node;
            ChangeNodeColor(node.holder, firstNodeColor);
        }
        else
        {
            node.holder.NodeColor = defaultNodeColor;
        }
        SelectNode = node.holder;
        editingTree.nodeList.Add(node);
    }
    public CommandNode LoadCommandNode(NodeSaveData _data, bool IsAITreeOnly = false)
    {
        CommandNode node;
        if (IsAITreeOnly)
        {
            node = CreateCommandNode(_data.programID, _data.programValues);
        }
        else
        {
            node = CreateNodeAndUI(_data.localPos, false, _data.programID, _data.programValues);
        }
        return node;
    }


    //UI上でのedgeを作成
    EdgeOnUI CreateEdgeOnUI()
    {
        var edgeUI = Instantiate(commandEdgePre, Vector3.zero, Quaternion.identity, commandBackPanel.transform).GetComponent<EdgeOnUI>();
        edgeUI.aITreeGenerator = this;
        edgeUI.transform.localPosition = Vector3.zero;
        edgeUI.transform.SetAsFirstSibling();
        return edgeUI;
    }
    //AIのedgeを作成
    CommandEdge CreateCommandEdge(int checkerID, List<int> checkerValues = null, CommandNode pre_node = null, CommandNode next_node = null)
    {
        var edge = new CommandEdge
        {
            checker = EdgeDataBase.GetInstance().EdgeDataList[checkerID].CreateCheckerInstance()
        };
        if (pre_node != null)
        {
            edge.AddPreNode(pre_node);
        }
        if (next_node != null)
        {
            edge.AddNextNode(next_node);
        }
        if (edge.checker != null && checkerValues != null)
        {
            var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var fields = edge.checker.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(edge.checker, checkerValues[i]);
            }
        }
        return edge;
    }
    //UI上でのedgeとAIのedgeを作成
    CommandEdge CreateEdgeAndUI(int checkerID, List<int> checkerValues = null, CommandNode pre_node = null, CommandNode next_node = null)
    {
        var edgeui = CreateEdgeOnUI();
        edgeui.CommandEdge = CreateCommandEdge(checkerID, checkerValues, pre_node, next_node);
        return edgeui.CommandEdge;
    }
    void DeleteEdge(EdgeOnUI _edge)
    {
        if (_edge)
        {
            editingTree.edgeList.Remove(_edge.CommandEdge);
            _edge.Sucide();
            if (SelectEdge == _edge) SelectEdge = null;
        }
    }
    public void DeleteSelectEdge()
    {
        DeleteEdge(SelectEdge);
    }
    public void CreateDefaultEdge()
    {
        if (SelectNode)
        {
            var edge = CreateEdgeAndUI(0, null, SelectNode.CommandNode);
            SelectEdge = edge.holder;
            editingTree.edgeList.Add(edge);
        }
    }
    public CommandEdge LoadCommandEdge(EdgeSaveData _data, MechAITree ai_tree, bool IsAITreeOnly = false)
    {
        CommandEdge edge;
        if (IsAITreeOnly)
        {
            edge = CreateCommandEdge(_data.checkrID, _data.checkerValues, ai_tree.nodeList[_data.preCommandNumber], ai_tree.nodeList[_data.nextCommandNumber]);
        }
        else
        {
            edge = CreateEdgeAndUI(_data.checkrID, _data.checkerValues, ai_tree.nodeList[_data.preCommandNumber], ai_tree.nodeList[_data.nextCommandNumber]);
        }
        return edge;
    }
}
