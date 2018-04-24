using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNode
{
    public NodeOnUI holder;
    public NodeActivity activity = null;
    public List<CommandEdge> edges = new List<CommandEdge>();
    public  void DeleteMe()
    {
        //自分を参照している全てのedgeのthisに対してnullを代入
        foreach (var i in edges)
        {
            if (i.pre == this) i.pre = null;
            if (i.next == this) i.next = null;
        }
        edges.Clear();
    }
}
