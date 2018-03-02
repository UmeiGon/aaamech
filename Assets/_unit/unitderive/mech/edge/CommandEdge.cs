using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEdge
{
    public Transform holder;
    public Command pre = null;
    public Command next = null;
    public EdgeChecker nextChecker = null;
    public EdgeChecker preChecker = null;
    public virtual bool Check() { return false; }

    public void DeleteMe()
    {
        if (pre != null) pre.edges.Remove(this);
        if (next != null) next.edges.Remove(this);
        pre = null;
        next = null;
    }
    //自分のpreかnextが自分(edge)を持っていなかった場合追加する。
    public void CommandAddMe()
    {
        if (pre != null&& pre.edges.Find(x => x == this)==null)
        {
            pre.edges.Add(this);
        }
        if (next != null && next.edges.Find(x => x == this) == null)
        {
            next.edges.Add(this);
        }
    }
    //追加に成功した場合真を返す
    public bool AddPreNode(Command c_node)
    {
        //preが既にある場合は追加不可能
        if (pre != null)
        {
            return false;
        }
        //nextが既にある場合
        if (next != null)
        {
            //同じ関係性のedgeが無ければpreに
            if (next.edges.Find(x => x.pre == c_node || x.next == c_node) == null)
            {
                pre = c_node;
                c_node.edges.Add(this);
                return true;
            }
        }
        else
        {
            pre = c_node;
            c_node.edges.Add(this);
            return true;
        }
        return false;
    }
    //追加に成功した場合真を返す
    public bool AddNextNode(Command c_node)
    {
        //preが既にある場合は追加不可能
        if (next != null)
        {
            return false;
        }
        //preが既にある場合
        if (pre != null)
        {
            //同じ関係性のedgeが無ければnextに
            if (pre.edges.Find(x => x.pre == c_node || x.next == c_node) == null)
            {
                next = c_node;
                c_node.edges.Add(this);
                return true;
            }
        }
        else
        {
            next = c_node;
            c_node.edges.Add(this);
            return true;
        }
        return false;
    }
}
