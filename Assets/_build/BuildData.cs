using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildData
{
    public BuildID BuildId
    {
        private set;
        get;
    }
    public string BuildName
    {
        private set;
        get;
    }
    //key...itemID,value...個数
    public Dictionary<int, int> consumptionItemData = null;
    System.Type builderType;
    public GameObject ObjPre
    {
        get;
        private set;
    }
    public static BuildData CreateBuildData<BuilderType>(BuildID build_id, string build_name, Dictionary<int, int> consumption_items)
        where BuilderType : BuilderControllerSuperClass, new()
    {
        var bd = new BuildData()
        {
            BuildId = build_id,
            BuildName = build_name,
            consumptionItemData = consumption_items,
            ObjPre = Resources.Load(build_name) as GameObject,
        };
        bd.builderType = typeof(BuilderType);
        return bd;
    }
    public BuilderControllerSuperClass CreateBuilderControllerInstance()
    {
        return (BuilderControllerSuperClass)System.Activator.CreateInstance(builderType);
    }
    private BuildData() { }
}