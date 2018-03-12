using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData
{
    public string nodeName;
    public System.Type type;
    public NodeData(string node_name, System.Type _type)
    {
        nodeName = node_name;
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
    public CommandProgram GetProgramInstance(int programId)
    {
        return nodeDataList[Mathf.Clamp(programId, 0, nodeDataList.Count - 1)].GetProgramInstance();
    }
    public int FindTypeNumber(CommandProgram _command)
    {
        if (_command == null) return 0;
        int n = 0;
        foreach (var i in nodeDataList)
        {
            if (i.type==_command.GetType())
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
            new NodeData("敵を探して攻撃",typeof(AttackProgram)),
            new NodeData("採集する",typeof(PickUpItemProgram)),
            new NodeData("建築する",typeof(BuildProgram)),
            new NodeData("ホームに帰る",typeof(ReturnBaseProgram)),
        };
    }
}
