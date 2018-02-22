using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommandManager : MonoBehaviour
{
  
    [SerializeField]
    GameObject commandBackPanel;
    [SerializeField]
    GameObject commandNodePre;
    [SerializeField]
    GameObject commandEdgePre;
    [SerializeField]
    GameObject createEdgePopPanel;
    [SerializeField]
    GameObject deleteEdgePopPanel;
    EdgeOnUI selectEdgeReal;
    NodeOnUI selectNodeReal;
    EdgeOnUI SelectEdge { get { return selectEdgeReal; }set {
            if (selectEdgeReal != null)
            {
                selectEdgeReal.NonSelectTrigger();
            }
            selectEdgeReal = value;
            if (selectEdgeReal != null)
            {
                selectEdgeReal.SelectTrigger();
            }
        }
    }
    NodeOnUI SelectNode { get {return selectNodeReal; }set {
            if (selectNodeReal != null)
            {
                selectNodeReal.NonSelectTrigger();
            }
            selectNodeReal = value;
            if (selectNodeReal != null)
            {
                selectNodeReal.SelectTrigger();
            }
        }
    }
    public void CreateCommandNode()
    {

        var c = Instantiate(commandNodePre, new Vector3(0, 0, 0), new Quaternion(), commandBackPanel.transform);
        c.transform.localPosition = new Vector3(50, -50, 0);
        c.transform.SetAsLastSibling();
        c.GetComponent<NodeOnUI>().cManager = this;

    }
    public void DeleteEdgePopUp(EdgeOnUI _edge)
    {
        SelectEdge = _edge;
        deleteEdgePopPanel.SetActive(true);
    } 

    public void NodeRightClick(NodeOnUI _node)
    {
        SelectNode = _node;
        createEdgePopPanel.SetActive(true);
    }
    public void NodeLeftClick(NodeOnUI _node)
    {
        //nodeにedgeを付ける
        if (SelectNode && SelectNode != _node&& SelectEdge)
        {
            if (SelectEdge.commandEdge.AddNextNode(_node.commandNode))
            {
                SelectNode = null;
                SelectEdge = null;
            }  
        }
    }
    public void DeleteCommandEdge()
    {
        //自分を持ってるノードのedgelistに対してremoveする
        deleteEdgePopPanel.SetActive(false);
        SelectEdge.commandEdge.pre.edges.Remove(SelectEdge.commandEdge);
        SelectEdge.commandEdge.next.edges.Remove(SelectEdge.commandEdge);
        Destroy(SelectEdge.gameObject);
    }
    public void CreateCommandEdge()
    {

        if (SelectNode)
        {
            createEdgePopPanel.SetActive(false); 
            SelectEdge = Instantiate(commandEdgePre,Vector3.zero,new Quaternion(),commandBackPanel.transform).GetComponent<EdgeOnUI>();
            SelectEdge.cManager = this;
            SelectEdge.transform.localPosition = Vector3.zero;
            SelectEdge.commandEdge.AddPreNode(SelectNode.commandNode);
            SelectEdge.transform.SetAsFirstSibling();
        }
    }
   
}
