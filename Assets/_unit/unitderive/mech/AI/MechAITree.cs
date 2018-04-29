using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MechAITree
{
    bool uiIsActive=true;
    public bool UIIsActive
    {
        get { return uiIsActive; }
        set
        {
            uiIsActive = value;
            foreach (var n in nodeList)
            {
                if(n.holder) n.holder.gameObject.SetActive(uiIsActive);
            }
            foreach (var e in edgeList)
            {
                if(e.holder)e.holder.gameObject.SetActive(uiIsActive);
            }
        }
    }
    public List<CommandNode> nodeList = new List<CommandNode>();
    public List<CommandEdge> edgeList = new List<CommandEdge>();
    public CommandNode firstNode;
}
