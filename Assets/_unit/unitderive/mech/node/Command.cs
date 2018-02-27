using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public Transform holder;
    public CommandProgram program = null;
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
    //nextかpreに既にあるedgeの場合trueを返す。
    //bool PreEdgeCheck(CommandEdge _edge)
    //{
    //    return (preEdges.Find(x => x.pre == _edge.pre) != null);
    //}
    //bool NextEdgeCheck(CommandEdge _edge)
    //{
    //    return (preEdges.Find(x => x.pre == _edge.pre) != null);
    //}

}
