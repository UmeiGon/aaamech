using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NodeDataBase
{
    static NodeDataBase instance;
    public static NodeDataBase GetInstance()
    {
        if (instance == null)
        {
            instance = new NodeDataBase();
        }
        return instance;
    }
    public List<NodeData> NodeDataList
    {
        get;
        private set;
    }
    public Dictionary<System.Type,NodeData> NodeDataDictionary
    {
        get;
        private set;
    }
    public NodeData FindNodeData<T>()
        where T : NodeActivity
    {
        return NodeDataDictionary[typeof(T)];
    }
    public NodeActivity CreateAcitivityInstance(int _acitivityId)
    {
        return NodeDataList[Mathf.Clamp(_acitivityId, 0, NodeDataList.Count - 1)].GetActivityInstance();
    }
    public int FindTypeNumber(NodeActivity _command)
    {
        if (_command == null) return 0;
        int n = 0;
        foreach (var i in NodeDataList)
        {
            if (i.ActivityType == _command.GetType())
            {
                return n;
            }
            n++;
        }
        return -1;
    }
    NodeDataBase()
    {
        NodeDataList = new List<NodeData>()
        {
           NodeData.CreateNodeData<AttackNodeActivity>("敵攻撃","指定の敵を攻撃"),
           NodeData.CreateNodeData<PickUpItemNodeActivity>("素材収集","指定した素材を収集"),
           NodeData.CreateNodeData<BuildNodeActivity>("建築","一番近い建物を建築する"),
           NodeData.CreateNodeData<ReturnBaseNodeActivity>("帰還","一番近いホームに帰る"),
        };
        NodeDataDictionary = NodeDataList.ToDictionary(x=>x.ActivityType);
    }
}
