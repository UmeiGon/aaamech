using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEdge
{
    public EdgeOnUI holder;
    public CommandNode pre = null;
    public CommandNode next = null;
    public EdgeChecker checker = null;
    public virtual bool Check() { return false; }

    public void ReferenceRemoving()
    {
        if (pre != null) pre.NextStreamEdges.Remove(this);
        if (next != null) next.NextStreamEdges.Remove(this);
        pre = null;
        next = null;
    }
    //自分のpreかnextが自分(edge)を持っていなかった場合追加する。
    public void CommandAddMe()
    {
        if (pre != null&& pre.NextStreamEdges.Find(x => x == this)==null)
        {
            pre.NextStreamEdges.Add(this);
        }
        if (next != null && next.NextStreamEdges.Find(x => x == this) == null)
        {
            next.NextStreamEdges.Add(this);
        }
    }
    //追加に成功した場合真を返す
    public bool AddPreNode(CommandNode c_node)
    {
        //preが既にある場合は追加不可能
        if (pre != null)
        {
            return false;
        }
        //nextが既にある場合
        if (next != null)
        {
            //既に同じ関係性のedgeが無ければpreに
            if (next.NextStreamEdges.Find(x => x.pre == c_node) == null)
            {
                pre = c_node;
                c_node.NextStreamEdges.Add(this);
                return true;
            }
        }
        else
        {
            pre = c_node;
            c_node.NextStreamEdges.Add(this);
            return true;
        }
        return false;
    }
    //追加に成功した場合真を返す
    public bool AddNextNode(CommandNode c_node)
    {
        //nextが既にある場合は追加不可能
        if (next != null)
        {
            return false;
        }

        if (pre != null)
        {
            //既に同じ関係性のedgeが無ければnextに
            if (pre.NextStreamEdges.Find(x => x.next == c_node) == null)
            {
                next = c_node;
                c_node.PreStreamEdges.Add(this);
                return true;
            }
        }
        else
        {
            next = c_node;
            c_node.PreStreamEdges.Add(this);
            return true;
        }
        return false;
    }
}
