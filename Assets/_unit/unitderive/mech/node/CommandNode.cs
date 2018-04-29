using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNode
{
    public NodeOnUI holder;
    public NodeActivity activity = null;
    public List<CommandEdge> NextStreamEdges = new List<CommandEdge>();
    public List<CommandEdge> PreStreamEdges = new List<CommandEdge>();
    public void ReferenceRemoving()
    {
        //自分を参照している全てのedgeのthisに対してnullを代入
        foreach (var i in NextStreamEdges)
        {
            if (i.pre == this) i.pre = null;
            if (i.next == this) i.next = null;
        }
        foreach (var i in PreStreamEdges)
        {
            if (i.pre == this) i.pre = null;
            if (i.next == this) i.next = null;
        }
        NextStreamEdges.Clear();
        PreStreamEdges.Clear();
    }
}
