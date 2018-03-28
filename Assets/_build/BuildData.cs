using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildData
{
    BuildID buildId;
    //key...itemID,value...個数
    public Dictionary<int, int> consumptionItemData = null;
    public BuildData(BuildID build_id,Dictionary<int, int> consumption_items,BuilderSuperClass _builder)
    {
        buildId = build_id;
        consumptionItemData = consumption_items;
        builder = _builder;
    }
    BuilderSuperClass builder;
    void BuildingUpdate()
    {
        builder.BuilderUpdate();
    }
}