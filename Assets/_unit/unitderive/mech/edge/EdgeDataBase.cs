using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeData
{
    public string edgeName;
    public System.Type type;
    public EdgeData(string edge_name, System.Type _type)
    {
        edgeName = edge_name;
        type = _type;
    }
    public EdgeChecker GetCheckerInstance()
    {
        if (type == null)
        {
            return null;
        }
        return (EdgeChecker)System.Activator.CreateInstance(type);
    }
}
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
        foreach (var i in edgeDataList)
        {
            if (i.type == _edge.GetType())
            {
                return n;
            }
            n++;
        }
        return -1;
    }
    public List<EdgeData> edgeDataList;
    EdgeDataBase()
    {
        edgeDataList = new List<EdgeData>()
        {
            new EdgeData("攻撃されたとき",typeof(AttackedChecker)),
             new EdgeData("素材を採集した時",typeof(MaterialChecker)),
             new EdgeData("体力が数値以上か以下の時",typeof(HelthDamagedChecker)),
        };
    }
}
