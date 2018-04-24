using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeData
{
    public string EdgeName { get; private set; }
    public string EdgeDescription { get; private set; }
    public System.Type CheckerType { get; private set; }
    public static EdgeData CreateEdgeData<T>(string edge_name,string edge_description)
        where T : EdgeChecker
    {
        var retEdgeData = new EdgeData
        {
            EdgeName = edge_name,
            EdgeDescription= edge_description,
            CheckerType = typeof(T)
        };
        return retEdgeData;
    }
    public EdgeChecker CreateCheckerInstance()
    {
        if (CheckerType == null)
        {
            return null;
        }
        return (EdgeChecker)System.Activator.CreateInstance(CheckerType);
    }
}
