using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData
{
    public string NodeName { get; private set; }
    public string NodeDescription { get; private set; }
    public System.Type ActivityType { get; private set; }
    public static NodeData CreateNodeData<T>(string node_name,string node_description)
        where T:NodeActivity
    {
        var retNodeData = new NodeData()
        {
            NodeName = node_name,
            NodeDescription = node_description,
            ActivityType = typeof(T),
        };
        return retNodeData;
    }
    public NodeActivity GetActivityInstance()
    {
        if (ActivityType == null)
        {
            return null;
        }
        return (NodeActivity)System.Activator.CreateInstance(ActivityType);
    }
}
