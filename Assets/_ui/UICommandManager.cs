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
    UnityEngine.UI.Button createEdgeButton;
    [SerializeField]
    UnityEngine.UI.Button deleteEdgeButton;
    [SerializeField]
    UnityEngine.UI.Button deleteNodeButton;
    [SerializeField]
    Color defaultNodeColor;
    [SerializeField]
    Color firstNodeColor;
    MechAITree nowTree=null;
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
        deleteEdgeButton.interactable = false;
        deleteNodeButton.interactable = false;
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
    public void DeleteCommandNode()
    {
        if (SelectNode != null)
        {
            nowTree.commandList.Remove(SelectNode.commandNode);
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
        nodeui.cManager = this;
        nodeui.startLimitPosition = new Vector3(0, 0, 0);
        nodeui.endLimitPosition = commandBackPanel.GetComponent<RectTransform>().sizeDelta;
        nodeui.startLimitPosition.y *= -1;
        nodeui.endLimitPosition.y *= -1;
        nowTree.commandList.Add(nodeui.commandNode);
        //firstがまだ設定されて無かったらfirstに
        if (nowTree.firstCommand == null)
        {
            nowTree.firstCommand=nodeui.commandNode;
            nodeui.GetComponent<Image>().color = firstNodeColor;
        }
        else
        {
            nodeui.GetComponent<Image>().color = defaultNodeColor;
        }

    }
    public void SaveMechAI()
    {
        
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
        createEdgeButton.interactable = true;
        deleteNodeButton.interactable = true;
    }
    void NotSelectedNodeTrigger()
    {
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
