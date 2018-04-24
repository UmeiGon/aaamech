using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MechAITree
{
    public string treeName;
    bool isActive=true;
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            foreach (var n in nodeList)
            {
                n.holder.gameObject.SetActive(isActive);
            }
            foreach (var e in edgeList)
            {
                e.holder.gameObject.SetActive(isActive);
            }
        }
    }
    public List<CommandNode> nodeList = new List<CommandNode>();
    public List<CommandEdge> edgeList = new List<CommandEdge>();
    public Action firstChanged;
    public CommandNode firstNode;
}
