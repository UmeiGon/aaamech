using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUnit : Unit {
    [SerializeField]
    GameObject buildObject;
    public override void Death()
    {
        buildObject.SetActive(true);
        buildObject.transform.parent = null;
        //var navs= GameObject.Find("NavMeshSurface").transform;
        //buildObject.transform.parent = navs.transform;
        //navs.GetComponent<BuildNavMesh>().BuildNavMeshFunc();
        base.Death();
    }
}
