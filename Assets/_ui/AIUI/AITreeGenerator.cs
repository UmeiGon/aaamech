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
    Button goButton;
    [SerializeField]
    Text ironText;
    [SerializeField]
    Text consumeText;
    [SerializeField]
    Button resetButton;
    [SerializeField]
    Button saveButton;
    [SerializeField]
    Button loadButton;
    [SerializeField]
    GameObject blockingEditMask;
    ItemManager itemManager;
    MechGenerator mechGenerator;
    public MechAITree editingTree = null;
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
        goButton.interactable = false;
        goButton.onClick.AddListener(GenerateMech);

        resetButton.onClick.AddListener(ResetEditingTree);

        mechGenerator = CompornentUtility.FindCompornentOnScene<MechGenerator>();
        itemManager = CompornentUtility.FindCompornentOnScene<ItemManager>();

        consumeText.text = mechGenerator.ConsumeIronValue + "を消費してメカを作成";
        itemManager.itemDataTable[(int)ItemID.Iron].AddValueChangedTrigger(ChangeGoButton);
        ironText.text = itemManager.itemDataTable[(int)ItemID.Iron].Value.ToString();
        itemManager.itemDataTable[(int)ItemID.Iron].Value += 40;
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
    public void ChangeGoButton(int _value)
    {
        ironText.text = _value.ToString();
        goButton.interactable = (_value >= mechGenerator.ConsumeIronValue);
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
    void AloneDeleteEdge()
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
            ChangeNodeColor(SelectNode.commandNode.holder, defaultNodeColor);
            editingTree.firstNode = SelectNode.commandNode;
        }
    }

    void ChangeNodeColor(NodeOnUI firstCommand, Color _color)
    {
        firstCommand.NodeColor = _color;
    }

    public void GenerateMech()
    {
        if (editingTree.firstNode == null) return;
        mechGenerator.GenerateMech(editingTree);
        //editingTreeのコピーを作成して再度editingtreeに入れる(コピーコンストラクタの働き)
        var temp = PackageToAITree(AIPackage.TreeToPackage(editingTree));
        ResetEditingTree();
        editingTree = temp;
    }
    public void ResetEditingTree()
    {
        SelectEdge = null;
        SelectNode = null;
        foreach (var i in editingTree.nodeList)
        {
            Destroy(i.holder.gameObject);
        }
        foreach (var i in editingTree.edgeList)
        {
            Destroy(i.holder.gameObject);
        }
        editingTree = new MechAITree();
    }
    public void SaveEditingMechAI(string file_name = "dafault")
    {
        SaveMechAI(editingTree, file_name);
    }
    public void SaveMechAI(MechAITree ai_tree, string file_name = "dafault")
    {
        if (ai_tree.firstNode == null) return;
        AISaveLoad.GetInstance().SaveAITree(AIPackage.TreeToPackage(ai_tree), file_name);
    }
    public MechAITree LoadMechAI(string file_name = "default")
    {
        return PackageToAITree(AISaveLoad.GetInstance().LoadAITree(file_name));
    }

    //AIPackageをaitreeに変換
    MechAITree PackageToAITree(AIPackage ai_pack)
    {
        if (ai_pack == null) return new MechAITree();
        MechAITree retAI = new MechAITree();

        int n = 0;
        foreach (var c in ai_pack.nodeDataList)
        {
            var node = LoadCommandNode(c);
            retAI.nodeList.Add(node);
            //firstNodeだった場合は色を変える
            if (ai_pack.firstCommandID == n)
            {
                retAI.firstNode = node;
                ChangeNodeColor(retAI.firstNode.holder, firstNodeColor);
            }
            n++;
        }
        foreach (var e in ai_pack.edgeDataList)
        {
            var edge = LoadCommandEdge(e, retAI);
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
            if (!SelectEdge.commandEdge.AddNextNode(_node.commandNode))
            {
                SelectEdge.commandEdge.AddPreNode(_node.commandNode);
            }
        }
    }



    CommandNode CreateNode(Vector3 _pos, bool world_pos_flag, int acitivity_id = 0, List<int> acitivity_values = null)
    {
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var c = Instantiate(commandNodePre, new Vector3(0, 0, 0), new Quaternion(), commandBackPanel.transform);
        if (world_pos_flag)
        {
            c.transform.position = _pos;
        }
        else
        {
            c.transform.localPosition = _pos;
        }
        c.transform.SetAsLastSibling();
        var nodeui = c.GetComponent<NodeOnUI>();
        nodeui.commandNode.holder = nodeui;
        nodeui.aITreeGenerator = this;

        //リミット付ける
        nodeui.startLimitPosition = new Vector3(0, 0, 0);
        nodeui.endLimitPosition = commandBackPanel.GetComponent<RectTransform>().sizeDelta;
        nodeui.startLimitPosition.y *= -1;
        nodeui.endLimitPosition.y *= -1;

        //acitivityのインスタンスをゲットして,メンバ変数のデータも入力
        nodeui.commandNode.activity = NodeDataBase.GetInstance().CreateAcitivityInstance(acitivity_id);
        if ((nodeui.commandNode.activity = NodeDataBase.GetInstance().NodeDataList[acitivity_id].GetActivityInstance()) != null && acitivity_values != null)
        {
            var fields = nodeui.commandNode.activity.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(nodeui.commandNode.activity, acitivity_values[i]);
            }
        }
        return nodeui.commandNode;
    }
    public void DeleteSelectNode()
    {
        if (SelectNode == null) return;
        editingTree.nodeList.Remove(SelectNode.commandNode);
        if (SelectNode.commandNode == editingTree.firstNode)
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
        SelectNode.DeleteNode();
        Destroy(SelectNode.gameObject);
        SelectNode = null;
        AloneDeleteEdge();
    }
    public void CreateDefaultNode(Vector3 _pos)
    {
        var node = CreateNode(_pos, true);
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
    public CommandNode LoadCommandNode(NodeSaveData _data)
    {
        var node = CreateNode(_data.localPos, false, _data.programID, _data.programValues);
        ChangeNodeColor(node.holder, defaultNodeColor);
        return node;
    }


    CommandEdge CreateEdge(int checkerID, List<int> checkerValues = null, CommandNode pre_node = null, CommandNode next_node = null)
    {
        var edgeui = Instantiate(commandEdgePre, Vector3.zero, new Quaternion(), commandBackPanel.transform).GetComponent<EdgeOnUI>();
        edgeui.aITreeGenerator = this;
        edgeui.transform.localPosition = Vector3.zero;
        edgeui.commandEdge.holder = edgeui;
        if (pre_node != null)
        {
            edgeui.commandEdge.AddPreNode(pre_node);
        }
        if (next_node != null)
        {
            edgeui.commandEdge.AddNextNode(next_node);
        }
        var filedFlag = (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        //checkerデータ入力
        edgeui.commandEdge.checker = EdgeDataBase.GetInstance().EdgeDataList[checkerID].CreateCheckerInstance();
        if (checkerValues != null)
        {
            var fields = edgeui.commandEdge.checker.GetType().GetFields(filedFlag);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(edgeui.commandEdge.checker, checkerValues[i]);
            }
        }
        edgeui.transform.SetAsFirstSibling();
        return edgeui.commandEdge;
    }
    public void DeleteEdge(EdgeOnUI _edge)
    {
        if (_edge)
        {
            editingTree.edgeList.Remove(_edge.commandEdge);
            _edge.DeleteEdge();
            Destroy(_edge.gameObject);
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
            var edge = CreateEdge(0, null, SelectNode.commandNode);
            SelectEdge = edge.holder;
            editingTree.edgeList.Add(edge);
        }
    }
    public CommandEdge LoadCommandEdge(EdgeSaveData _data, MechAITree ai_tree)
    {
        return CreateEdge(_data.checkrID, _data.checkerValues, ai_tree.nodeList[_data.preCommandNumber], ai_tree.nodeList[_data.nextCommandNumber]);
    }
}
