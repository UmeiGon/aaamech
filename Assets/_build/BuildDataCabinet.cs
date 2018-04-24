using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildDataCabinet
{
    //シングルトン
    static BuildDataCabinet instance = null;
    public static BuildDataCabinet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuildDataCabinet();
            }
            return instance;
        }
    }
    public Dictionary<int, BuildData> buildDataTable;

    private BuildDataCabinet()
    {
        var buildDataList = new List<BuildData>
        {
            BuildData.CreateBuildData<RaycastBuilderController>(BuildID.Bridge,"Bridge",new Dictionary<int,int>{{(int)ItemID.Wood,20 } }),
            BuildData.CreateBuildData<LimitedPositonBuilderController>(BuildID.Tower,"HealTower",new Dictionary<int,int>{{(int)ItemID.Stone,30 } }),
        };
        //todictionaryでidをkeyにしたdictionaryを作る
        buildDataTable = buildDataList.ToDictionary(d => (int)d.BuildId);
    }
}
