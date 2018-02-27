using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData
{
    public string nodeName;
    public System.Type type;
    public NodeData(string edge_name, System.Type _type)
    {
        nodeName = edge_name;
        type = _type;
    }
    public CommandProgram GetProgramInstance()
    {
        if (type == null)
        {
            return null;
        }
        return (CommandProgram)System.Activator.CreateInstance(type);
    }
}
public class NodeDataBase
{
    static NodeDataBase inst;
    public static NodeDataBase GetInstance()
    {
        if (inst == null)
        {
            inst = new NodeDataBase();
        }
        return inst;
    }
    public List<NodeData> nodeDataList;
    public int FindTypeNumber(System.Type _type)
    {
        int n = 0;
        foreach (var i in nodeDataList)
        {
            if (i.type==_type)
            {
                return n;
            }
            n++;
        }
        return -1;
    }
    NodeDataBase()
    {
        nodeDataList = new List<NodeData>()
        {
            new NodeData("無し",null),
            new NodeData("敵を探して攻撃",typeof(AttackProgram)),
            new NodeData("採集する",typeof(PickUpItemProgram)),
            new NodeData("建築する",typeof(BuildProgram)),
        };
    }
}
