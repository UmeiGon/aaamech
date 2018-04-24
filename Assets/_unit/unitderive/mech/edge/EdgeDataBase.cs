using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EdgeDataBase
{
    static EdgeDataBase inst;
    public static EdgeDataBase GetInstance()
    {
        if (inst == null)
        {
            inst = new EdgeDataBase();
        }
        return inst;
    }
    public int FindTypeNumber(EdgeChecker _edge)
    {
        if (_edge == null) return 0;
        int n = 0;
        foreach (var i in EdgeDataList)
        {
            if (i.CheckerType == _edge.GetType())
            {
                return n;
            }
            n++;
        }
        return -1;
    }
    public EdgeData FindNodeData<T>()
      where T : EdgeChecker
    {
        return EdgeDataDictionary[typeof(T)];
    }
    public List<EdgeData> EdgeDataList { get; private set; }
    public Dictionary<System.Type, EdgeData> EdgeDataDictionary { get; private set; }
    EdgeDataBase()
    {
        EdgeDataList = new List<EdgeData>()
        {
              EdgeData.CreateEdgeData<ReceivedAttackEdgeChecker>("攻撃受け","攻撃を受けた時に遷移"),
              EdgeData.CreateEdgeData<HaveMaterialEdgeChecker>("素材個数","指定の素材が数値以上の時に遷移"),
              EdgeData.CreateEdgeData<HelthRateEdgeChecker>("体力割合","体力が指定した数値以上か以下の時に遷移"),
              EdgeData.CreateEdgeData<LoseTargetEdgeChecker>("標的ロスト","ターゲットしているユニットが無くなった時に遷移"),
        };
        EdgeDataDictionary = EdgeDataList.ToDictionary(x=>x.CheckerType);
    }
}
