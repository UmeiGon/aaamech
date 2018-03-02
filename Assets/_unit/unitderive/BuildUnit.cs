using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUnit : Unit {
    [SerializeField]
    GameObject buildObject;
    UnitLists unitlists;
 
    public override void Death()
    {
        buildObject.SetActive(true);
        buildObject.transform.parent = null;
        var unitLists=GameObject.Find("Parent").GetComponentInChildren<UnitLists>(); 
        //foreach (var i in unitLists.playerMechList)
        //{
        //    i.mechCon.agent.SetDestination(Vector3.zero);
        //}
        //var navs= GameObject.Find("NavMeshSurface").transform;
        //buildObject.transform.parent = navs.transform;
        //navs.GetComponent<BuildNavMesh>().BuildNavMeshFunc();
        base.Death();
    }
}
